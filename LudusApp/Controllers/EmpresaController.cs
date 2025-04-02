using LudusApp.Application.Dtos.Empresa;
using LudusApp.Application.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class EmpresaController : ControllerBase
{
    private readonly EmpresaService _service;

    public EmpresaController(EmpresaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<ReadEmpresaDto>>> BuscarTodas()
    {
        return Ok(await _service.BuscarTodas());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReadEmpresaDto>> BuscarPorId(Guid id)
    {
        try
        {
            return Ok(await _service.BuscarPorId(id));
        }
        catch (ApplicationException ex)
        {
            return NotFound(ex.Message);
        }
    }
    /// <summary>
    /// Cadastra empresa e vincula/cria usuário caso não exista
    /// </summary>
    /// <param name="empresaDto"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult> Cadastrar([FromBody] CreateEmpresaDto empresaDto)
    {
        var resposta = await _service.CadastrarOuVincularEmpresa(empresaDto);
        return Ok(resposta);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Atualizar(Guid id, UpdateEmpresaDto empresaDto)
    {
        try
        {
            await _service.Atualizar(id, empresaDto);
            return NoContent();
        }
        catch (ApplicationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    //[HttpDelete("{id}")]
    //public async Task<ActionResult> Deletar(Guid id)
    //{
    //    await _service.Deletar(id);
    //    return NoContent();
    //}
}