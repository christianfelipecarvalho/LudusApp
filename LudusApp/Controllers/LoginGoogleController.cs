using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

[Route("api/auth")]
[ApiController]
public class LoginGoogleController : ControllerBase
{
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
        // Log para verificar o início da resposta
        Console.WriteLine("Iniciando a autenticação com o Google.");

        var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        // Log para ver o que está acontecendo após a autenticação
        if (authenticateResult == null)
        {
            Console.WriteLine("Autenticação retornou null.");
            return BadRequest("Falha ao autenticar com Google: resposta nula.");
        }

        if (!authenticateResult.Succeeded)
        {
            // Log do erro de falha na autenticação
            Console.WriteLine("Falha na autenticação: " + authenticateResult.Failure?.Message);
            return BadRequest("Falha ao autenticar com Google");
        }

        var claims = authenticateResult.Principal?.Identities?.FirstOrDefault()?.Claims.Select(c => new { c.Type, c.Value });

        var user = new
        {
            GoogleId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
            FullName = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value,
            Email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
            ProfilePicture = claims.FirstOrDefault(c => c.Type == "picture")?.Value // Foto do perfil
        };


        // Log para verificar os claims
        if (claims == null)
        {
            Console.WriteLine("Nenhum claim encontrado.");
        }
        else
        {
            foreach (var claim in claims)
            {
                Console.WriteLine($"Claim: {claim.Type} - {claim.Value}");
            }
        }

        return Ok(new
        {
            Message = "Autenticação com Google bem-sucedida",
            Claims = claims
        });
    }


}
