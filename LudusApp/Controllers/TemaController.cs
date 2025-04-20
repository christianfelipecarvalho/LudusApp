using LudusApp.Application.DTOs;
using LudusApp.Application.Mapper;
using LudusApp.Application.Services;
using LudusApp.Domain.TemaSettings;
using Microsoft.AspNetCore.Mvc;


namespace LudusApp.Controllers;
/// <summary>
/// Controlador para alteração e busca de temas
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class TemaController : BaseController<TemaService, Tema, TemaReadDto, TemaReadDto, TemaReadDto>
{
    private readonly IMapper<Tema, TemaReadDto, TemaReadDto, TemaReadDto> _mapper;

    public TemaController(TemaService temaService, IMapper<Tema, TemaReadDto, TemaReadDto, TemaReadDto> mapper) :
        base(temaService)
    {
        _mapper = mapper;
    }

    /// <summary>
    /// Atualiza o tema do usuario
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] TemaReadDto temaReadDto)
    {
        try
        {
            var tema = _mapper.MapToUpdateEntity(temaReadDto);

            var resultado = await _service.AtualizarOuCriarTemaAsync(tema);
            var temaRetornoDTO = _mapper.MapToReadDto(resultado);

            return Ok(temaRetornoDTO);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }

    [HttpGet("usuario/{id}")]
    public async Task<IActionResult> BuscarPorIdUsuario(string id)
    {
        var tema = await _service.RecuperaPorIdUsuario(id);

        if (tema == null)
        {
            return NotFound($"Nenhum item encontrado para o ID: {id}");
        }

        return Ok(tema);
    }
}