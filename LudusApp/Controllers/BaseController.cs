using Microsoft.AspNetCore.Mvc;
using LudusApp.Application.Services;

namespace LudusApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController<TService, TEntity, TReadDto, TCreateDto, TUpdateDto> : ControllerBase
    where TService : BaseService<TEntity, TReadDto, TCreateDto, TUpdateDto>
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
        var resultado = await _service.RecuperaTodosAsync();

        if (resultado == null || !resultado.Any())
        {
            return NotFound("Nenhum item encontrado.");
        }

        return Ok(resultado);
    }
 
}