using System.Text.Json;
using LudusApp.Application.Dtos.Localidades;
using LudusApp.Application.Interfaces.ReadOnly.Localidade;
using LudusApp.Application.Mappers;
using LudusApp.Domain.Entities.Localidades;
using LudusApp.Domain.Entities.Localidades.Cidade;
using LudusApp.Domain.Entities.Localidades.Estado;
using LudusApp.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace LudusApp.Application.Services;

public class LocalidadeService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalidadeRepository _localidadeRepository;
    private readonly ILocalidadeReadRepository _localidadeReadRepository;
    private readonly ILogger<LocalidadeService> _logger;

    

    public LocalidadeService(HttpClient httpClient, ILocalidadeRepository localidadeRepository,
        ILocalidadeReadRepository localidadeReadRepository, ILogger<LocalidadeService> logger)
    {
        _httpClient = httpClient;
        _localidadeRepository = localidadeRepository;
        _localidadeReadRepository = localidadeReadRepository;
        _logger = logger;
    }

    public async Task SincronizarLocalidadesAsync()
    {
        try
        {
            _logger.LogInformation("Inicia SincronizarLocalidadesAsync");
            // Buscar estados
            var response = await _httpClient.GetAsync("https://servicodados.ibge.gov.br/api/v1/localidades/estados");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var estadosDto = JsonSerializer.Deserialize<List<EstadoDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            foreach (var estadoDto in estadosDto)
            {
                try
                {
                    var estado = new Estado
                    {
                        Id = estadoDto.Id,
                        Sigla = estadoDto.Sigla,
                        Nome = estadoDto.Nome
                    };

                    var estadoExistente = await _localidadeRepository.RecuperaPorIdIntAsync(estado.Id);

                    if (estadoExistente == null)
                    {
                        await _localidadeRepository.AddAsync(estado);
                    }
                    else if (estadoExistente.Nome != estado.Nome || estadoExistente.Sigla != estado.Sigla)
                    {
                        estadoExistente.Nome = estado.Nome;
                        estadoExistente.Sigla = estado.Sigla;
                        await _localidadeRepository.UpdateAsync(estadoExistente);
                    }

                    // Buscar cidades
                    var responseCidades = await _httpClient.GetAsync(
                        $"https://servicodados.ibge.gov.br/api/v1/localidades/estados/{estado.Sigla}/municipios");
                    responseCidades.EnsureSuccessStatusCode();

                    var jsonCidades = await responseCidades.Content.ReadAsStringAsync();
                    var cidadesDto = JsonSerializer.Deserialize<List<CidadeDto>>(jsonCidades, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    foreach (var cidadeDto in cidadesDto)
                    {
                        try
                        {
                            var cidadeExistente = await _localidadeRepository.ObterCidadesPorId(cidadeDto.Id);
                            if (cidadeExistente == null)
                            {
                                var cidade = new Cidade
                                {
                                    Id = cidadeDto.Id,
                                    Nome = cidadeDto.Nome,
                                    EstadoId = estado.Id
                                };

                                await _localidadeRepository.AddCidadeAsync(cidade);
                            }
                            else if (cidadeExistente.Nome != cidadeDto.Nome)
                            {
                                cidadeExistente.Nome = cidadeDto.Nome;
                                await _localidadeRepository.UpdateCidadeAsync(cidadeExistente);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Erro ao processar cidade. Detalhes: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Erro ao processar estado. Detalhes: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro geral ao sincronizar localidades. Detalhes: {ex.Message}");
        }
    }

    #region Estado

    public async Task<List<EstadoDto>> ObterTodosEstadosAsync()
    {
        _logger.LogInformation("Iniciando busca todos os estados");
        var estados = await _localidadeRepository.RecuperaTodosAsync();
        return estados.Select(EstadoMapper.ToDto).ToList();
    }

    public async Task<EstadoDto> ObterEstadoPorIdAsync(int id)
    {
        _logger.LogInformation("Iniciando busca de estado por id");
        var estado = await _localidadeRepository.RecuperaPorIdIntAsync(id);
        if (estado == null)
        {
            throw new Exception($"Estado com ID {id} não foi encontrado.");
        }

        return EstadoMapper.ToDto(estado);
    }

    public async Task<List<EstadoDto>> ObterEstadoPorNome(string nome)
    {
        var estado = await _localidadeRepository.BuscarEstadosPorNomeAsync(nome);

        return estado.Select(EstadoMapper.ToDto).ToList();
    }

    #endregion

    #region Cidade

    public async Task<List<CidadeDto>> ObterCidadesComPaginacaoAsync(int pagina, int tamanhoPagina)
    {
        var cidades = await _localidadeReadRepository.ObterCidadesComEstadoPaginadoAsync(pagina, tamanhoPagina);

        if (cidades == null || !cidades.Any()) 
            return new List<CidadeDto>();

        return cidades.Select(c => new CidadeDto
        {
            Id = c.Id,
            Nome = c.Nome,
            EstadoId = c.EstadoId,
            EstadoNome = c.EstadoNome
        }).ToList();
    }

    public async Task<CidadeDto> ObterCidadePorIdAsync(int id)
    {
        var cidade = await _localidadeReadRepository.RecuperaPorIdTipoInt(id);

        if (cidade == null)
        {
            throw new KeyNotFoundException($"Cidade com ID {id} não encontrada.");
        }

        var estado = await _localidadeRepository.RecuperaPorIdIntAsync(cidade.EstadoId);

        return new CidadeDto
        {
            Id = cidade.Id,
            Nome = cidade.Nome,
            EstadoId = cidade.EstadoId,
            EstadoNome = estado?.Nome
        };
    }


    public async Task<List<CidadeDto>> ObterCidadePorNomeAsync(string nome)
    {
        var cidades = await _localidadeRepository.BuscarCidadesPorNomeAsync(nome);

        if (cidades == null || !cidades.Any())
        {
            throw new KeyNotFoundException($"Nenhuma cidade encontrada com o nome {nome}.");
        }

        return cidades.Select(c => new CidadeDto
        {
            Id = c.Id,
            Nome = c.Nome,
            EstadoId = c.EstadoId,
            EstadoNome = _localidadeRepository.RecuperaPorIdIntAsync(c.EstadoId).Result?.Nome ?? throw new InvalidOperationException()
        }).ToList();
    }

    public async Task<List<CidadeDto>> ObterCidadePeloIdEstadoAsync(int idEstado, int pagina, int tamanhoPagina)
    {
        var cidades = await _localidadeReadRepository.ObterCidadesPorEstadoIdAsync(idEstado, pagina, tamanhoPagina);

        if (cidades == null || !cidades.Any())
        {
            throw new KeyNotFoundException($"Nenhuma cidade encontrada com estadoId = {idEstado}.");
        }

        return cidades.Select(c => new CidadeDto
        {
            Id = c.Id,
            Nome = c.Nome,
            EstadoId = c.EstadoId,
            EstadoNome = c.EstadoNome
        }).ToList();
    }

    public async Task<List<CidadeDto>> ObterCidadePeloNomeEstadoAsync(string nomeEstado, int pagina, int tamanhoPagina)
    {
        var cidades =
            await _localidadeReadRepository.ObterCidadesPorEstadoPeloNomeAsync(nomeEstado, pagina, tamanhoPagina);

        if (cidades == null || !cidades.Any())
        {
            throw new KeyNotFoundException($"Nenhuma cidade encontrada com o Estado = {nomeEstado}.");
        }

        return cidades.Select(c => new CidadeDto
        {
            Id = c.Id,
            Nome = c.Nome,
            EstadoId = c.EstadoId,
            EstadoNome = c.EstadoNome
        }).ToList();
    }

    #endregion

    #region CEP

    public async Task<BuscaCepDto> BuscarPorCepAsync(string cep)
    {
        if (string.IsNullOrWhiteSpace(cep) || cep.Length != 8)
        {
            throw new ArgumentException("CEP inválido.");
        }

        var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Erro ao buscar informações do CEP na API externa.");
        }

        var cepData = JsonSerializer.Deserialize<ViaCepDto>(await response.Content.ReadAsStringAsync(),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (cepData == null || string.IsNullOrWhiteSpace(cepData.Localidade) || string.IsNullOrWhiteSpace(cepData.Uf))
        {
            throw new Exception("Informações do CEP não encontradas.");
        }

        var cidade = (await _localidadeRepository.BuscarCidadesPorNomeAsync(cepData.Localidade)).FirstOrDefault();

        if (cidade == null)
        {
            throw new KeyNotFoundException($"Cidade não encontrada para o nome: {cepData.Localidade}");
        }

        var estado = await _localidadeRepository.BuscarEstadoPorSiglaAsync(cepData.Uf);
        if (estado == null)
        {
            throw new KeyNotFoundException($"Estado não encontrado para a UF: {cepData.Uf}");
        }

        return new BuscaCepDto
        {
            Cep = cepData.Cep,
            Logradouro = cepData.Logradouro,
            Complemento = cepData.Complemento,
            Bairro = cepData.Bairro,
            CidadeId = cidade.Id,
            CidadeNome = cidade.Nome,
            EstadoId = estado.Id,
            EstadoNome = estado.Nome
        };
    }

    #endregion
}