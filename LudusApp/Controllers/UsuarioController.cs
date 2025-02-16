using LudusApp.Application.Dtos.Usuario;
using LudusApp.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LudusApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{

    private readonly UsuarioService _usuarioService;

    public UsuarioController(UsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUsuarioDto loginUsuarioDto)
    {
        var token = await _usuarioService.Login(loginUsuarioDto);
        return Ok(new { access_token = token });
    }

    [HttpPost("cadastro")]
    [Authorize]
    public async Task<IActionResult> CadastraUsuario([FromBody] CreateUsuarioDto usuarioDto)
    {
        await _usuarioService.Cadastra(usuarioDto);
        return Ok("Usuário cadastrado!");

    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> EditaUsuario([FromBody] UpdateUsuarioDto usuarioDto)
    {
        await _usuarioService.Edita(usuarioDto);
        return Ok("Usuário Editado com sucesso!");
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> BuscarTodosUsuarios([FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        var usuarios = await _usuarioService.BuscarTodos(skip, take);

        if (!usuarios.Any())
            return NoContent();

        return Ok(usuarios);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> BuscaUsuarioPorId(string id)
    {
        var usuario = await _usuarioService.BuscaPorId(id);
        return Ok(usuario);
    }

    [HttpPatch("{id}")]
    [Authorize]
    public async Task<IActionResult> AtualizarParcialmente(string id, [FromBody] JsonPatchDocument<UpdateCampoUsuarioDto> patchDocument)
    {
        if (patchDocument == null)
        {
            return BadRequest("Documento de patch não pode ser nulo.");
        }

        var usuarioAtualizado = await _usuarioService.AtualizarParcial(id, patchDocument);

        if (usuarioAtualizado == null)
        {
            return NotFound("Usuário não encontrado.");
        }

        return Ok(usuarioAtualizado);
    }
}
