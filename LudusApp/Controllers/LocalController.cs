using System.ComponentModel;
using LudusApp.Application.Dtos.Local;
using LudusApp.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LudusApp.Controllers;

/// <summary>
/// Controlador responsavel pelo gerenciamento de locais/quadras
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class LocalController : ControllerBase
{
    private readonly LocalService _localService;

    public LocalController(LocalService localService)
    {
        _localService = localService;
    }

    /// <summary>
    /// Adiciona local
    /// </summary>
    /// <param name="localDto"></param>
    /// <returns></returns>
    [Authorize(Policy = "SomenteEmpresa")]
    [HttpPost]
    public async Task<IActionResult> AdicionarLocal([FromBody] LocalCreateDto localDto)
    {
        var locais = await _localService.AddAsync(localDto);

        return Ok("Local adicionado com sucesso!");
    }

    /// <summary>
    /// Atualiza um local
    /// </summary>
    /// <param name="id"></param>
    /// <param name="localDto"></param>
    /// <returns></returns>
    [Authorize(Policy = "SomenteEmpresa")]
    [HttpPut("{id}")]
    public async Task<IActionResult> AlterarLocal(Guid id, [FromBody] LocalUpdateDto localDto)
    {
        var local = await _localService.AtualizaAsync(id, localDto);

        return Ok(local);
    }

    /// <summary>
    /// Busca todos os locais com paginação default 10 ou o que for definido
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

        var locais = await _localService.ObterLocaisComPaginacao(pagina, tamanhoPagina);

        return Ok(locais);
    }

    /// <summary>
    /// Busca o local por id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarPorId(Guid id)
    {
        var local = await _localService.RecuperaPorIdAsync(id);
        return Ok(local);
    }
}