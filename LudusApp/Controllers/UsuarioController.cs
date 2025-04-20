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
public class UsuarioController : ControllerBase
{

    private readonly UsuarioService _usuarioService;

    public UsuarioController(UsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    /// <summary>
    /// Cadastra um novo usuário.
    /// </summary>
    /// <param name="usuarioDto">Dados do usuário.</param>
    /// <returns>Mensagem de sucesso.</returns>
    [HttpPost("cadastro")]
    //[Authorize]
    public async Task<IActionResult> CadastraUsuario([FromBody] CreateUsuarioDto usuarioDto)
    {
        await _usuarioService.Cadastra(usuarioDto);
        return Ok("Usuário cadastrado!");

    }

    /// <summary>
    /// Edita um usuário existente.
    /// </summary>
    /// <param name="usuarioDto">Novos dados do usuário.</param>
    /// <returns>Mensagem de sucesso.</returns>
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> EditaUsuario([FromBody] UpdateUsuarioDto usuarioDto)
    {
        await _usuarioService.Edita(usuarioDto);
        return Ok("Usuário Editado com sucesso!");
    }

    /// <summary>
    /// Busca todos os usuários cadastrados.
    /// </summary>
    /// <param name="skip">Quantidade de usuários a serem ignorados (paginação).</param>
    /// <param name="take">Quantidade de usuários a serem retornados.</param>
    /// <returns>Lista de usuários.</returns>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> BuscarTodosUsuarios([FromQuery] int skip = 0, [FromQuery] int take = 50)
    {
        var usuarios = await _usuarioService.BuscarTodos(skip, take);

        if (!usuarios.Any())
            return NoContent();

        return Ok(usuarios);
    }

    /// <summary>
    /// Busca usuário por id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Dados do usuario</returns>
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> BuscaUsuarioPorId(string id)
    {
        var usuario = await _usuarioService.BuscaPorId(id);
        return Ok(usuario);
    }

    /// <summary>
    /// Atualiza parcialmente um usuário.
    /// </summary>
    /// <param name="id">ID do usuário.</param>
    /// <param name="patchDocument">Dados a serem atualizados.</param>
    /// <returns>Usuário atualizado.</returns>
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
    
    [HttpPost("redefinir-senha")]
    public async Task<IActionResult> RedefinirSenha([FromBody] RedefinirSenhaUsuarioDto dto)
    {
        try
        {
            var msg = await _usuarioService.RedefinirSenha(dto.UserId, dto.Token, dto.NovaSenha);
            return Ok(msg);
        }
        catch (ApplicationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

}
