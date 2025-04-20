using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using AutoMapper;
using LudusApp.Application.Dtos.Empresa;
using LudusApp.Application.Dtos.Usuario;
using LudusApp.Application.Mapper;
using LudusApp.Domain.Entities.Emails;
using LudusApp.Domain.Services;
using LudusApp.Domain.Usuarios;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace LudusApp.Application.Services;

public class UsuarioService
{
    private readonly IMapper _mapper;
    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _signInManager;
    private readonly TokenService _tokenService;
    private readonly UsuarioDomainService _usuarioDomainService;
    private readonly EmailService _emailService;
    private readonly TemaService _temaService;

    public UsuarioService(UserManager<Usuario> userManager, IMapper mapper, SignInManager<Usuario> signInManager, TokenService tokenService, UsuarioDomainService usuarioDomainService, EmailService emailService, TemaService temaService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _usuarioDomainService = usuarioDomainService;
        _emailService = emailService;
        _temaService = temaService;
    }

    public async Task Cadastra(CreateUsuarioDto usuarioDto, Guid? empresaId = null)
    {
        // Verifica duplicidades de email, username e CPF em uma única consulta
        var duplicados = await _userManager.Users
            .AsNoTracking()
            .Where(u => u.Email == usuarioDto.Email || u.UserName == usuarioDto.UserName || u.Cpf == usuarioDto.Cpf)
            .ToListAsync();

        if (duplicados.Any())
        {
            // Identifica qual campo está duplicado
            if (duplicados.Any(u => u.Email == usuarioDto.Email))
            {
                throw new ApplicationException("Já existe um usuário com este e-mail.");
            }
            if (duplicados.Any(u => u.UserName == usuarioDto.UserName))
            {
                throw new ApplicationException("Já existe um usuário com este username.");
            }
            if (duplicados.Any(u => u.Cpf == usuarioDto.Cpf))
            {
                throw new ApplicationException("Já existe um usuário com este CPF.");
            }
        }

        // Convertendo DTO para entidade e associando à empresa, se fornecido
        Usuario usuario = usuarioDto.ToEntity();
        usuario.EmpresaId = empresaId;

        // Cria o usuário usando o UserManager
        var resultado = await _userManager.CreateAsync(usuario, usuarioDto.Password);

        if (!resultado.Succeeded)
        {
            string erros = string.Join(", ", resultado.Errors.Select(e => e.Description));
            throw new ApplicationException($"Falha ao cadastrar usuário: {erros}");
        }
        
        await _temaService.CriarUsuarioComTemaPadrao(usuario.Id);
        
        // Enviar e-mail de boas-vindas
        var substituicoes = new Dictionary<string, string>
    {
        { "Nome", usuario.Nome },
        { "Email", usuario.Email }
    };

        await _emailService.EnviarEmailAsync(configuracaoId: new Guid("00000000-0000-0000-0000-000000000001"), // ID da configuração de email padrão
                                             destinatario: usuario.Email,
                                             tipoTemplate: "BoasVindasUsuario",
                                             substituicoes: substituicoes);

    }



    public async Task Edita(UpdateUsuarioDto usuarioDto, Guid? empresaId = null)
    {
        var usuarioExistente = await _userManager.FindByIdAsync(usuarioDto.Id.ToString());

        if (usuarioExistente == null)
        {
            throw new ApplicationException("Usuário não encontrado.");
        }

        var duplicados = await _userManager.Users
            .Where(u => (u.Email == usuarioDto.Email || u.Cpf == usuarioDto.Cpf) && u.Id != usuarioDto.Id.ToString())
            .ToListAsync();

        if (duplicados.Any())
        {
            if (duplicados.Any(u => u.Email == usuarioDto.Email))
            {
                throw new ApplicationException("Já existe um usuário com este e-mail.");
            }
            if (duplicados.Any(u => u.Cpf == usuarioDto.Cpf))
            {
                throw new ApplicationException("Já existe um usuário com este CPF.");
            }
        }

        if (usuarioExistente.Nome != usuarioDto.Nome)
        {
            usuarioExistente.Nome = usuarioDto.Nome;
        }

        if (usuarioExistente.Email != usuarioDto.Email)
        {
            usuarioExistente.Email = usuarioDto.Email;
        }

        if (usuarioExistente.Cpf != usuarioDto.Cpf)
        {
            usuarioExistente.Cpf = usuarioDto.Cpf;
        }

        if (usuarioExistente.EmpresaId != empresaId)
        {
            usuarioExistente.EmpresaId = empresaId;
        }

        // Atualiza os dados do usuário
        var resultado = await _userManager.UpdateAsync(usuarioExistente);

        if (!resultado.Succeeded)
        {
            string erros = string.Join(", ", resultado.Errors.Select(e => e.Description));
            throw new ApplicationException($"Falha ao atualizar usuário: {erros}");
        }
    }

    public async Task<List<ReadUsuarioDto>> BuscarTodos(int skip, int take, Guid? tenantId = null)
    {
        var query = _userManager.Users.AsQueryable();

        if (!string.IsNullOrEmpty(tenantId.ToString()))
        {
            query = query.Where(u => u.TenantId == tenantId);
        }

        var usuarios = await query
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        return usuarios.Select(u => u.ToReadDto()).ToList();
    }
    public async Task<ReadUsuarioDto> BuscaPorId(string id, Guid? tenantId = null)
    {
        var usuarioExistente = await _userManager.FindByIdAsync(id);

        if (usuarioExistente == null || usuarioExistente.IsMultiTenant && usuarioExistente.TenantId != tenantId)
        {
            throw new ApplicationException("Usuário não encontrado ou acesso negado.");
        }

        return usuarioExistente.ToReadDto();
    }
    public async Task<Usuario> AtualizarParcial(string id, JsonPatchDocument<UpdateCampoUsuarioDto> patchDocument)
    {
        var usuarioExistente = await _userManager.FindByIdAsync(id);

        if (usuarioExistente == null)
        {
            return null; // Ou pode lançar uma exceção se preferir
        }

        // Mapeia o usuário existente para o DTO
        var usuarioDto = usuarioExistente.ToUpdateCampoDto();

        // Aplica as alterações do patch no DTO
        patchDocument.ApplyTo(usuarioDto);

        // Valida manualmente o DTO atualizado
        var validationContext = new ValidationContext(usuarioDto);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(usuarioDto, validationContext, validationResults, true);

        if (!isValid)
        {
            foreach (var validationResult in validationResults)
            {
                throw new ApplicationException($"Falha na validação: {validationResult.ErrorMessage}");
            }
        }

        // Mapeia as alterações do DTO de volta para o modelo
        usuarioExistente.UpdateFromUpdateCampoDto(usuarioDto);

        // Atualiza o usuário no Identity
        var resultado = await _userManager.UpdateAsync(usuarioExistente);

        if (!resultado.Succeeded)
        {
            string erros = string.Join(", ", resultado.Errors.Select(e => e.Description));
            throw new ApplicationException($"Falha ao atualizar usuário: {erros}");
        }

        return usuarioExistente;
    }


    public async Task<Object> Login(LoginUsuarioDto loginUsuarioDto)
    {
        // Buscar o usuário com base no CPF, username ou e-mail
        //Usuario usuario = Regex.IsMatch(loginUsuarioDto.Identificador, @"^\d{3}\.\d{3}\.\d{3}-\d{2}$") == true ? _signInManager.UserManager.Users.FirstOrDefault(user => user.Cpf == loginUsuarioDto.Identificador) : _signInManager.UserManager.Users.FirstOrDefault(user => user.UserName == loginUsuarioDto.Identificador);
        Usuario usuario = loginUsuarioDto.Identificador.Contains("@") ?  _signInManager.UserManager.Users.FirstOrDefault(user => user.UserName == loginUsuarioDto.Identificador) :  _signInManager.UserManager.Users.FirstOrDefault(user => user.Cpf == loginUsuarioDto.Identificador);

        if (usuario == null)
        {
            throw new ApplicationException("Usuário não encontrado.");
        }

        // Autentica o usuário com username e senha
        var resultado = await _signInManager.PasswordSignInAsync(usuario.UserName, loginUsuarioDto.Password, true, false); // Mudar para false se não for usar cookies

        if (!resultado.Succeeded)
        {
            throw new ApplicationException("Usuário e/ou senha inválidos.");
        }

        // Gera o token JWT
        var token = _tokenService.GenerateToken(usuario);

        return new { access_token = token };
    }

    public async Task<string> LoginComGoogle(string googleId, string email, string fullName)
    {
        var usuario = await _userManager.Users.FirstOrDefaultAsync(u => u.GoogleId == googleId);

        if (usuario == null)
        {
            // Se o usuário já existir no banco com esse e-mail, associamos o GoogleId
            usuario = await _userManager.FindByEmailAsync(email);
            if (usuario != null)
            {
                usuario.GoogleId = googleId;
                await _userManager.UpdateAsync(usuario);
            }
            else
            {
                usuario = new Usuario
                {
                    UserName = email,
                    Email = email,
                    Nome = fullName,
                    DataNascimento = DateTime.MinValue,
                    Cpf = "000.000.000-00",
                    GoogleId = googleId,
                };

                var resultado = await _userManager.CreateAsync(usuario);
                if (!resultado.Succeeded)
                {
                    throw new ApplicationException("Falha ao cadastrar usuário com Google.");
                }
            }
        }

        // Gerar token JWT para autenticação
        var token = _tokenService.GenerateToken(usuario);
        return token;
    }

    public async Task<string> CriarUsuarioComEmpresaDtoAsync(CreateEmpresaDto empresaDto)
    {
        // Criar a entidade do usuário
        var novoUsuario = new Usuario
        {
            Nome = empresaDto.NomeUsuario,
            UserName = empresaDto.Email,
            Email = empresaDto.Email,
            Cpf = empresaDto.cpfUsuario,
            DataNascimento = empresaDto.DataNascimentoUsuario,
            TenantId = empresaDto.TenantId
        };

        // Gerar senha temporária
        var resultado = await _userManager.CreateAsync(novoUsuario, empresaDto.SenhaTemporaria);

        if (!resultado.Succeeded)
        {
            throw new ApplicationException($"Erro ao criar usuário: {string.Join(", ", resultado.Errors.Select(e => e.Description))}");
        }
        return novoUsuario.Id;
    }
    public async Task<string> AdicionarTenantAoUsuarioAsync(string usuarioId, Guid? tenantId)
    {
        var usuario = await _userManager.FindByIdAsync(usuarioId);

        if (usuario == null)
        {
            throw new ApplicationException("Usuário não encontrado.");
        }

        usuario.TenantId = tenantId;
        var resultado = await _userManager.UpdateAsync(usuario);

        if (!resultado.Succeeded)
        {
            string erros = string.Join(", ", resultado.Errors.Select(e => e.Description));
            throw new ApplicationException($"Erro ao atualizar usuário: {erros}");
        }
        return usuarioId;
    }

}
