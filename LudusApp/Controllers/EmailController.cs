using LudusApp.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace LudusApp.Controllers;

/// <summary>
/// Cotrolador para e-mails de Saudação, confirmação, recuperação, e avisos em geral
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly UsuarioService _usuarioService;
    private readonly EmailService _emailService;

    public EmailController(UsuarioService usuarioService, EmailService emailService)
    {
        _usuarioService = usuarioService;
        _emailService = emailService;
    }
    /// <summary>
    /// Solicita confirmação de e-mail
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    [HttpPost("solicitar-confirmacao")]
    public async Task<IActionResult> SolicitarConfirmacaoEmail([FromBody] string email)
    {
        var resultado = await _emailService.SolicitarConfirmacaoEmail(email);
        if (!string.IsNullOrEmpty(resultado))
            return Ok(resultado);

        return BadRequest("Não foi possível enviar o e-mail de confirmação.");
    }
    
    /// <summary>
    /// Confirma e-mail 
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet("confirmar")]
    public async Task<IActionResult> ConfirmarEmail(string userId, string token)
    {
        var user = await _emailService.ConfirmaEmail(userId, token);
        return Ok(user);
    }
    /// <summary>
    /// Solicitar recuperacao de senha
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    [HttpPost("solicitar-recuperacao")]
    public async Task<IActionResult> SolicitarRecuperacaoSenha([FromBody] string email)
    {
        var resultado = await _emailService.SolicitarRecuperacaoSenha(email);
        return Ok(resultado);
    }


}