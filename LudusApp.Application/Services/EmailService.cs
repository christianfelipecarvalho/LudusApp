using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using LudusApp.Domain.Interfaces.Email;
using LudusApp.Domain.Entities.Emails;
using LudusApp.Domain.Enums;
using LudusApp.Domain.Usuarios;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace LudusApp.Application.Services;

public class EmailService
{
    private readonly ITemplateEmailRepository _templateRepository;
    private readonly IConfiguracaoEmailRepository _configuracaoRepository;
    private readonly IEmailRespository _emailRespository;
    private readonly UserManager<Usuario> _userManager;
    private readonly IConfiguration _configuration;



    public EmailService(ITemplateEmailRepository templateRepository,
        IConfiguracaoEmailRepository configuracaoRepository, IEmailRespository emailRespository, UserManager<Usuario> userManager, IConfiguration configuration)
    {
        _templateRepository = templateRepository;
        _configuracaoRepository = configuracaoRepository;
        _emailRespository = emailRespository;
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task EnviarEmailAsync(Guid configuracaoId, string destinatario, string tipoTemplate,
        Dictionary<string, string> substituicoes)
    {
        var configuracao = await _configuracaoRepository.RecuperaPorIdAsync(configuracaoId);
        if (configuracao == null)
        {
            throw new ApplicationException("Configuração de email não encontrada.");
        }

        var template = (await _templateRepository.FindAsync(t => t.Tipo == tipoTemplate)).FirstOrDefault();
        if (template == null)
        {
            throw new ApplicationException($"Template de email do tipo '{tipoTemplate}' não encontrado.");
        }

        var mensagemPersonalizada = template.Mensagem;
        foreach (var substituicao in substituicoes)
        {
            mensagemPersonalizada = mensagemPersonalizada.Replace($"{{{substituicao.Key}}}", substituicao.Value);
        }

        var email = new Email
        {
            Destinatario = destinatario,
            Assunto = template.Assunto,
            Mensagem = mensagemPersonalizada,
            Status = EnumStatusEmail.NAOENVIADO,
            DataEnvio = DateTime.UtcNow
        };

        try
        {
            // Configurar o SMTP e enviar o email
            var smtpClient = new SmtpClient(configuracao.ServidorSMTP)
            {
                Port = configuracao.Porta,
                Credentials = new NetworkCredential(configuracao.RemetenteEmail, configuracao.Senha),
                EnableSsl = configuracao.EnableSSL
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(configuracao.RemetenteEmail),
                Subject = template.Assunto,
                Body = mensagemPersonalizada,
                IsBodyHtml = true
            };

            mailMessage.To.Add(destinatario);

            await smtpClient.SendMailAsync(mailMessage);

            // Atualizar status e salvar no banco
            email.Status = EnumStatusEmail.ENVIADO;
        }
        catch (Exception ex)
        {
            // Se houver erro, registra na entidade
            email.Status = EnumStatusEmail.ERROR;
            email.DataErro = DateTime.UtcNow;

            // Log do erro (caso tenha um serviço de log)
            Console.WriteLine($"Erro ao enviar e-mail: {ex.Message}");
        }

        await _emailRespository.AddAsync(email);
    }
    
    public async Task<string> ConfirmaEmail(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        
        if (user == null) throw new ApplicationException("Usuário não encontrado");

        var result = await _userManager.ConfirmEmailAsync(user, token);
        
        if (result.Succeeded)
            return  "E-mail confirmado com sucesso!";

        return "Falha ao confirmar e-mail";
    }

    public async Task<string> SolicitarConfirmacaoEmail(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            throw new ApplicationException("Usuário não encontrado.");

        if (await _userManager.IsEmailConfirmedAsync(user))
            return "E-mail já confirmado anteriormente.";
        
        var urlBase = _configuration["Email:UrlBaseConfirmacao"];
            
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        
        var confirmationLink = $"{urlBase}/api/Email/Confirmar?userId={user.Id}&token={Uri.EscapeDataString(token)}";

        var substituicoes = new Dictionary<string, string>
        {
            { "Nome", user.Nome },
            { "Email", user.Email },
            { "Link", confirmationLink }
        };

        await EnviarEmailAsync(
            configuracaoId: new Guid("00000000-0000-0000-0000-000000000001"),
            destinatario: user.Email,
            tipoTemplate: "ConfirmacaoEmail",
            substituicoes: substituicoes);

        return "Solicitação de confirmação enviada com sucesso!";
    }

    public async Task<string> SolicitarRecuperacaoSenha(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            throw new ApplicationException("Usuário não encontrado.");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var urlBase = _configuration["Email:UrlBaseRecuperacaoSenha"]; // ex: https://localhost:3000/resetar-senha
        var link = $"{urlBase}?userId={user.Id}&token={Uri.EscapeDataString(token)}";

        var substituicoes = new Dictionary<string, string>
        {
            { "Nome", user.Nome },
            { "Link", link }
        };

        await EnviarEmailAsync(
            configuracaoId: new Guid("00000000-0000-0000-0000-000000000001"),
            destinatario: user.Email,
            tipoTemplate: "RecuperacaoSenha",
            substituicoes: substituicoes
        );

        return "Solicitação de recuperação enviada!";
    }

}