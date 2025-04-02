using Microsoft.AspNetCore.Mvc;
using LudusApp.Application.Services;

namespace LudusApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController<TService, TEntity> : ControllerBase
    where TService : BaseService<TEntity>
    where TEntity : class
{
    protected readonly TService _service;

    public BaseController(TService service)
    {
        _service = service;
    }

    /// <summary>
    /// Busca todos os itens (ex.: estados ou cidades)
    /// </summary>
    /// <returns>Lista de itens</returns>
    [HttpGet("Buscartodos")]
    public async Task<IActionResult> BuscarTodos()
    {
        var resultado = await _service.ObterTodosAsync();

        if (resultado == null || !resultado.Any())
        {
            return NotFound("Nenhum item encontrado.");
        }

        return Ok(resultado);
    }

    /// <summary>
    /// Busca item por ID
    /// </summary>
    /// <param name="id">ID do item</param>
    /// <returns>Busca todos os dados do Id correspondente</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> BuscarPorId(int id)
    {
        var item = await _service.ObterPorIdIntAsync(id);

        if (item == null)
        {
            return NotFound($"Nenhum item encontrado para o ID: {id}");
        }

        return Ok(item); // Retorna o objeto inteiro
    }

 
}