﻿using LudusApp.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LudusApp.Controllers;

/// <summary>
/// Controlador para gerencimaneto de localidades estado,cidades e ativação de sincronização de estados e cidades pelo IBGE
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class LocalidadesController : ControllerBase
{
    private readonly LocalidadeService _localidadeService;

    public LocalidadesController(LocalidadeService localidadeService)
    {
        _localidadeService = localidadeService;
    }

    #region EstadoController

    /// <summary>
    /// Ativa a busca de estados e cidades na API do IBGE
    /// </summary>
    /// <returns>Retorna Ok quando disparado o evento assincrono</returns>
    [Authorize]
    [HttpGet("sincronizar")]
    public async Task<IActionResult> Sincronizar()
    {
        await _localidadeService.SincronizarLocalidadesAsync();
        return Ok("Sincronização concluída com sucesso!");
    }

    /// <summary>
    /// Busca todos os estados do Brasil
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet("api/estado/BuscarTodos")]
    public async Task<IActionResult> BuscarTodosEstados()
    {
        var resultado = await _localidadeService.ObterTodosEstadosAsync();

        if (resultado == null || !resultado.Any())
        {
            return NotFound($"Erro na busca dos estados! Não foi encontrado nenhum. ");
        }

        return Ok(resultado);
    }

    /// <summary>
    /// Busca o estado por id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet("api/estado/id/{id}")]
    public async Task<IActionResult> BuscarEstadoPorId(int id)
    {
        var localidade = await _localidadeService.ObterEstadoPorIdAsync(id);

        if (localidade is null)
        {
            return NotFound($"Nenhuma localidade encontrada para o ID: {id}");
        }

        return Ok(localidade);
    }

    /// <summary>
    /// Busca estado por nome
    /// </summary>
    /// <param name="nome"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet("api/estado/nome/{nome}")]
    public async Task<IActionResult> BuscarEstadoPorNome(string nome)
    {
        var localidade = await _localidadeService.ObterEstadoPorNome(nome);

        if (localidade is null)
        {
            return null;
        }

        return Ok(localidade);
    }

    #endregion


    #region CidadeController

    /// <summary>
    /// Busca todas as cidades com paginação default 50 ou o que for definido
    /// </summary>
    /// <param name="pagina"></param>
    /// <param name="tamanhoPagina"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet("api/cidades/BuscarTodas")]
    public async Task<IActionResult> BuscaTodasCidades([FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 50)
    {
       
        pagina = Math.Max(pagina, 1);
        tamanhoPagina = Math.Max(tamanhoPagina, 10);

        var buscaCidade = await _localidadeService.ObterCidadesComPaginacaoAsync(pagina, tamanhoPagina);

        return Ok(buscaCidade);
    }

    /// <summary>
    /// Busca o cidade por id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet("api/cidade/{id}")]
    public async Task<IActionResult> BuscarCidadePorId(int id)
    {
        var localidade = await _localidadeService.ObterCidadePorIdAsync(id);

        if (localidade is null)
        {
            return NotFound($"Nenhuma localidade encontrada para o ID: {id}");
        }

        return Ok(localidade);
    }

    /// <summary>
    /// Busca de cidade por nome
    /// </summary>
    /// <param name="nome"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet("api/cidade/nome/{nome}")]
    public async Task<IActionResult> BuscarCidadePorNome(string nome)
    {
        var localidade = await _localidadeService.ObterCidadePorNomeAsync(nome);

        if (localidade is null)
        {
            return null;
        }

        return Ok(localidade);
    }

    /// <summary>
    /// Busca pelo nome do estado e traz todas as cidades
    /// </summary>
    /// <param name="nome"></param>
    /// <param name="pagina"></param>
    /// <param name="tamanhoPagina"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet("api/cidade/Estado/nome/{nome}")]
    public async Task<IActionResult> BuscarCidadePeloNomeDoEstado(string nome, int pagina = 1, int tamanhoPagina = 50)
    {
        var localidade = await _localidadeService.ObterCidadePeloNomeEstadoAsync(nome, pagina, tamanhoPagina);

        if (localidade is null)
        {
            return null;
        }

        return Ok(localidade);
    }

    /// <summary>
    /// Busca pelo id do estado e traz todas as cidades
    /// </summary>
    /// <param name="idEstado"></param>
    /// <param name="pagina"></param>
    /// <param name="tamanhoPagina"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet("api/cidade/Estado/id/{idEstado}")]
    public async Task<IActionResult> BuscarCidadePeloIdDoEstado(int idEstado, int pagina = 1, int tamanhoPagina = 50)
    {
        var localidade = await _localidadeService.ObterCidadePeloIdEstadoAsync(idEstado, pagina, tamanhoPagina);

        if (localidade is null)
        {
            return null;
        }

        return Ok(localidade);
    }

    #endregion

    /// <summary>
    /// Busca de dados de endereço por CEP
    /// </summary>
    /// <param name="cep"></param>
    /// <returns></returns>
    [Authorize]
    [HttpGet("api/cep/{cep}")]
    public async Task<IActionResult> BuscarPorCep(string cep)
    {
        try
        {
            var resultado = await _localidadeService.BuscarPorCepAsync(cep);

            return Ok(resultado);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno ao buscar informações do CEP: {ex.Message}");
        }
    }
}