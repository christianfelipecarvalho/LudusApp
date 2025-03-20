using LudusApp.Application.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

[Route("api/auth")]
[ApiController]
public class LoginGoogleController : ControllerBase
{
    private readonly UsuarioService _usuarioService;

    public LoginGoogleController(UsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet("google-login")]
    public IActionResult GoogleLogin()
    {
        var redirectUrl = Url.Action(nameof(GoogleResponse), "LoginGoogleController");
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("google-response")]
    public async Task<IActionResult> GoogleResponse()
    {
        var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        if (authenticateResult == null || !authenticateResult.Succeeded)
        {
            return BadRequest("Falha ao autenticar com Google");
        }

        var claims = authenticateResult.Principal?.Identities?.FirstOrDefault()?.Claims;

        if (claims == null)
        {
            return BadRequest("Nenhuma informação de usuário encontrada.");
        }

        var googleId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value; // ID único do Google
        var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value; // E-mail
        var nome = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value; // Nome completo
        var sobrenome = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value; // Sobrenome
        var nomeUsuario = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value; // Primeiro nome
        var fotoPerfil = claims.FirstOrDefault(c => c.Type == "picture")?.Value; // URL da foto de perfil
       
        
        /*
         * Verificar como liberar esses dados
         *  var localizacao = claims.FirstOrDefault(c => c.Type == "locale")?.Value; // Localidade (ex: "pt-BR")
            var perfilGoogle = claims.FirstOrDefault(c => c.Type == "profile")?.Value; // URL do perfil do Google
            var provedorAutenticacao = claims.FirstOrDefault(c => c.Type == ClaimTypes.AuthenticationMethod)?.Value; // Método de autenticação
            var telefone = claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone)?.Value; // Número de telefone
            var genero = claims.FirstOrDefault(c => c.Type == ClaimTypes.Gender)?.Value; // Gênero
            var dataNascimento = claims.FirstOrDefault(c => c.Type == ClaimTypes.DateOfBirth)?.Value; // Data de nascimento
            var fusoHorario = claims.FirstOrDefault(c => c.Type == "timezone")?.Value; // Fuso horário
            var endereco = claims.FirstOrDefault(c => c.Type == ClaimTypes.StreetAddress)?.Value; // Endereço
            var pais = claims.FirstOrDefault(c => c.Type == ClaimTypes.Country)?.Value; // País
            var atualizadoEm = claims.FirstOrDefault(c => c.Type == "updated_at")?.Value; // Última atualização do perfil
            var emailVerificado = claims.FirstOrDefault(c => c.Type == "email_verified")?.Value; // E-mail verificado (true/false)
            var hd = claims.FirstOrDefault(c => c.Type == "hd")?.Value; // Domínio do e-mail (ex: google.com se for do Google Workspace)

        */
        if (string.IsNullOrEmpty(email))
        {
            return BadRequest("O Google não retornou um e-mail válido.");
        }

        var token = await _usuarioService.LoginComGoogle(googleId, email, nome);

        return Ok(new
        {
            Message = "Autenticação com Google bem-sucedida",
            Token = token
        });
    }

}
