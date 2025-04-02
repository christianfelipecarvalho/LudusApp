using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using LudusApp.Domain.Interfaces.Email;
using LudusApp.Domain.Entities.Emails;
using LudusApp.Domain.Enums;

namespace LudusApp.Application.Services;

public class EmailService
{
    private readonly ITemplateEmailRepository _templateRepository;
    private readonly IConfiguracaoEmailRepository _configuracaoRepository;
    private readonly IEmailRespository _emailRespository;

    public EmailService(ITemplateEmailRepository templateRepository,
        IConfiguracaoEmailRepository configuracaoRepository, IEmailRespository emailRespository)
    {
        _templateRepository = templateRepository;
        _configuracaoRepository = configuracaoRepository;
        _emailRespository = emailRespository;
    }

    public async Task EnviarEmailAsync(Guid configuracaoId, string destinatario, string tipoTemplate,
        Dictionary<string, string> substituicoes)
    {
        var configuracao = await _configuracaoRepository.ObterPorIdAsync(configuracaoId);
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
}