using LudusApp.Application.Dtos;
using LudusApp.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LudusApp.Controllers;
/// <summary>
/// Controlador para registro de evendo, busca por evento e busca de eventos por local.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EventoController : ControllerBase
{
    private readonly EventoService _eventoService;

    public EventoController(EventoService eventoService)
    {
        _eventoService = eventoService;
    }
    /// <summary>
    /// Adiciona evento
    /// </summary>
    /// <param name="localDto"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AdicionarLocal([FromBody] EventoCreateDto eventoDto)
    {
        var eventos = await _eventoService.AddAsync(eventoDto);

        return Ok("Evento adicionado com sucesso!");
    }
    
    /// <summary>
    /// Atualiza um evento
    /// </summary>
    /// <param name="id"></param>
    /// <param name="localDto"></param>
    /// <returns></returns>
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> AlterarLocal(Guid id, [FromBody] EventoUpdateDto eventoDto)
    {
        var evento = await _eventoService.AtualizaAsync(id, eventoDto);

        return Ok(evento);
    }
    
    
    /// <summary>
    /// Busca todos os com paginação default 10 ou o que for definido
    /// </summary>
    /// <param name="pagina"></param>
    /// <param name="tamanhoPagina"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet("BuscarTodas")]
    public async Task<IActionResult> BuscaTodos([FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 10)
    {
        pagina = Math.Max(pagina, 1);
        tamanhoPagina = Math.Max(tamanhoPagina, 10);

        var eventos = await _eventoService.RecuperaTodosComPaginacaoAsync(pagina, tamanhoPagina);

        return Ok(eventos);
    }
    
    /// <summary>
    /// Busca o evento por id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarPorId(Guid id)
    {
        var evento = await _eventoService.RecuperaPorIdAsync(id);
        return Ok(evento);
    }
    
    /// <summary>
    /// Busca o evento por id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet("loca/{id}")]
    public async Task<IActionResult> BuscarPorIdLocal(Guid id)
    {
        var evento = await _eventoService.ObterEventoPorLocal(id);
        return Ok(evento);
    }

    /*
     *
     *  * VERIFICAR SE DEVEMOS CRIAR O EXCLUIR EVENTO OU INATIVAR
     *
     *
     *
     *
     * VERIFICAR SE DEVEMOS CRIAR O EXCLUIR EVENTO OU INATIVAR
     */
    
}