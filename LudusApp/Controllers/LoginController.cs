using LudusApp.Application.Dtos.Usuario;
using LudusApp.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LudusApp.Controllers;

/// <summary>
/// Controlador para gerenciar usuários.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{

    private readonly UsuarioService _usuarioService;

    public LoginController(UsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    /// <summary>
    /// Autentica um usuário e retorna um token JWT.
    /// </summary>
    /// <param name="loginUsuarioDto">Objeto com e-mail e senha do usuário.</param>
    /// <returns>Token de acesso.</returns>
    [HttpPost]
    public async Task<IActionResult> Login(LoginUsuarioDto loginUsuarioDto)
    {
        var token = await _usuarioService.Login(loginUsuarioDto);

        return Ok(token);
    }
}
