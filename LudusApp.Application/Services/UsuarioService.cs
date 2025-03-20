using System.ComponentModel.DataAnnotations;
using AutoMapper;
using LudusApp.Application.Dtos.Usuario;
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

    public UsuarioService(UserManager<Usuario> userManager, IMapper mapper, SignInManager<Usuario> signInManager, TokenService tokenService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    public async Task Cadastra(CreateUsuarioDto usuarioDto)
    {
        var usuarioExistente = await _userManager.FindByNameAsync(usuarioDto.UserName);

        if (usuarioExistente != null)
        {
            throw new ApplicationException("Usuário já existe.");
        }
        Usuario usuario = _mapper.Map<Usuario>(usuarioDto);
        IdentityResult resultado = await _userManager.CreateAsync(usuario, usuarioDto.Password);

        if (!resultado.Succeeded)
        {
            string erros = string.Join(", ", resultado.Errors.Select(e => e.Description));
            throw new ApplicationException($"Falha ao cadastrar usuário: {erros}");
        }
    }
    public async Task Edita(UpdateUsuarioDto usuarioDto)
    {
        var usuarioExistente = await _userManager.FindByIdAsync(usuarioDto.Id.ToString());

        if (usuarioExistente == null)
        {
            throw new ApplicationException("Usuário não encontrado.");
        }
        _mapper.Map(usuarioDto, usuarioExistente);
        IdentityResult resultado = await _userManager.UpdateAsync(usuarioExistente);

        if (!resultado.Succeeded)
        {
            string erros = string.Join(", ", resultado.Errors.Select(e => e.Description));
            throw new ApplicationException($"Falha ao atualizar usuário: {erros}");
        }
    }

    public async Task<List<ReadUsuarioDto>> BuscarTodos(int skip, int take)
    {
        var usuarios = await _userManager.Users
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        return _mapper.Map<List<ReadUsuarioDto>>(usuarios);
    }
    public async Task<ReadUsuarioDto> BuscaPorId(string id)
    {
        var usuarioExistente = await _userManager.FindByIdAsync(id);

        if (usuarioExistente == null)
        {
            throw new ApplicationException("Usuário não encontrado.");
        }

        return _mapper.Map<ReadUsuarioDto>(usuarioExistente);
    }
    public async Task<Usuario> AtualizarParcial(string id, JsonPatchDocument<UpdateCampoUsuarioDto> patchDocument)
    {
        var usuarioExistente = await _userManager.FindByIdAsync(id);

        if (usuarioExistente == null)
        {
            return null; // Ou pode lançar uma exceção se preferir
        }

        // Mapeia o usuário existente para o DTO para aplicar o patch
        var usuarioDto = _mapper.Map<UpdateCampoUsuarioDto>(usuarioExistente);

        // Aplica as alterações do patch no DTO
        patchDocument.ApplyTo(usuarioDto);

        // Agora, você pode simular a validação manualmente
        var validationContext = new ValidationContext(usuarioDto);
        var validationResults = new List<ValidationResult>();
        bool isValid = Validator.TryValidateObject(usuarioDto, validationContext, validationResults, true);

        if (!isValid)
        {
            foreach (var validationResult in validationResults)
            {
                // Exemplo: lançar uma exceção com todos os erros de validação
                throw new ApplicationException($"Falha na validação: {validationResult.ErrorMessage}");
            }
        }

        // Mapeia as mudanças de volta para o modelo de usuário
        _mapper.Map(usuarioDto, usuarioExistente);

        // Tenta atualizar o usuário no Identity
        var resultado = await _userManager.UpdateAsync(usuarioExistente);

        if (!resultado.Succeeded)
        {
            string erros = string.Join(", ", resultado.Errors.Select(e => e.Description));
            throw new ApplicationException($"Falha ao atualizar usuário: {erros}");
        }

        return usuarioExistente;
    }



    public async Task<string> Login(LoginUsuarioDto loginUsuarioDto)
    {
        var resultado = await _signInManager.PasswordSignInAsync(loginUsuarioDto.UserName, loginUsuarioDto.Password, true, false); // mudar para false se não for utilizar o salvamento automatico no cookies do navegador

        if (!resultado.Succeeded)
        {
            throw new ApplicationException("Usuario e senha inválidos!");
        }
        var usuario = _signInManager.UserManager.Users.FirstOrDefault(user => user.NormalizedUserName == loginUsuarioDto.UserName.ToUpper());

        _tokenService.GenerateToken(usuario);

        var token = _tokenService.GenerateToken(usuario);

        return token;
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
                    Nome= fullName,
                    DataNascimento = DateTime.MinValue, 
                    Cpf = "000.000.000-00",
                    GoogleId = googleId,
                    ativo = true
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




}
