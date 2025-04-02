using LudusApp.Application.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

/// <summary>
/// Faz o login social com o google
/// </summary>
[Route("api/auth")]
[ApiController]
public class LoginGoogleController : ControllerBase
{
    private readonly UsuarioService _usuarioService;

    public LoginGoogleController(UsuarioService usuarioService)

    {
        _usuarioService = usuarioService;
    }

    /// <summary>
    /// Inicia o processo de autenticação com o Google.
    /// Redireciona o usuário para o Google para efetuar login.
    /// </summary>
    /// <returns>Redireciona para a página de autenticação do Google.</returns>
    [HttpGet("google-login")]
    [ProducesResponseType(StatusCodes.Status302Found)]
    public IActionResult GoogleLogin()
    {
        var redirectUrl = Url.Action(nameof(GoogleResponse), "LoginGoogleController");
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }


    /// <summary>
    /// Recebe a resposta do Google após a autenticação e retorna um token JWT.
    /// </summary>
    /// <returns>Um token JWT gerado após a autenticação com o Google.</returns>
    /// <response code="200">Autenticação bem-sucedida e token gerado.</response>
    /// <response code="400">Falha na autenticação ou dados ausentes.</response>
    [HttpGet("google-response")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]  // Sucesso na autenticação
    [ProducesResponseType(StatusCodes.Status400BadRequest)]  // Falha na autenticação
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
