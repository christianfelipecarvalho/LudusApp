using System.Text.Json;
using LudusApp.Application.Dtos.Localidades;
using LudusApp.Application.Mappers;
using LudusApp.Domain.Entities.Localidades;
using LudusApp.Domain.Interfaces;
using LudusApp.Infra.Data;

namespace LudusApp.Application.Services;

public class LocalidadeService
{
    private readonly HttpClient _httpClient;
    private readonly LudusAppContext _dbContext;
    private readonly ILocalidadeRepository _localidadeRepository;


    public LocalidadeService(HttpClient httpClient, LudusAppContext dbContext, ILocalidadeRepository localidadeRepository)
    {
        _httpClient = httpClient;
        _dbContext = dbContext;
        _localidadeRepository = localidadeRepository;

    }

    public async Task SincronizarLocalidadesAsync()
    {
        try
        {
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

                    var estadoExistente = await _dbContext.Estados.FindAsync(estado.Id);

                    if (estadoExistente == null)
                    {
                        _dbContext.Estados.Add(estado);
                    }
                    else if (estadoExistente.Nome != estado.Nome || estadoExistente.Sigla != estado.Sigla)
                    {
                        estadoExistente.Nome = estado.Nome;
                        estadoExistente.Sigla = estado.Sigla;
                    }

                    await _dbContext.SaveChangesAsync();

                    // Buscar cidades
                    var responseCidades = await _httpClient.GetAsync($"https://servicodados.ibge.gov.br/api/v1/localidades/estados/{estado.Sigla}/municipios");
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
                            var cidadeExistente = await _dbContext.Cidades.FindAsync(cidadeDto.Id);
                            if (cidadeExistente == null)
                            {
                                var cidade = new Cidade
                                {
                                    Id = cidadeDto.Id,
                                    Nome = cidadeDto.Nome,
                                    EstadoId = estado.Id
                                };

                                _dbContext.Cidades.Add(cidade);
                            }
                            else if (cidadeExistente.Nome != cidadeDto.Nome)
                            {
                                cidadeExistente.Nome = cidadeDto.Nome;
                            }

                            await _dbContext.SaveChangesAsync();


                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro ao processar cidade. Detalhes: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao processar estado. Detalhes: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro geral ao sincronizar localidades. Detalhes: {ex.Message}");
        }
    }
    public async Task<List<EstadoDto>> ObterTodosEstadosAsync()
    {
        var estados = await _localidadeRepository.ObterTodosAsync();
        return estados.Select(EstadoMapper.ToDto).ToList();
    }

    public async Task<EstadoDto> ObterEstadoPorIdAsync(int id)
    {
        var estado = await _localidadeRepository.ObterPorIdIntAsync(id);
        if (estado == null)
        {
            throw new Exception($"Estado com ID {id} não foi encontrado.");
        }
        return EstadoMapper.ToDto(estado);
    }


    public async Task<List<CidadeDto>> ObterCidadesComPaginacaoAsync(int pagina, int tamanhoPagina)
    {
        var cidades = await _localidadeRepository.ObterCidadesComPaginacaoAsync(pagina, tamanhoPagina);
        return cidades.Select(CidadeMapper.ToDto).ToList();
    }
    public async Task<List<EstadoDto>> ObterEstadoPorNome(string nome)
    {
        var estado = await _localidadeRepository.BuscarEstadosPorNomeAsync(nome);

        return estado.Select(EstadoMapper.ToDto).ToList();
    }
    public async Task<CidadeDto> ObterCidadePorIdAsync(int id)
    {
        var cidade = await _localidadeRepository.ObterCidadesPorId(id);

        if (cidade == null)
        {
            throw new KeyNotFoundException($"Cidade com ID {id} não encontrada.");
        }

        // Buscar o estado associado
        var estado = await _localidadeRepository.ObterPorIdIntAsync(cidade.EstadoId);

        return new CidadeDto
        {
            Id = cidade.Id,
            Nome = cidade.Nome,
            EstadoId = cidade.EstadoId,
            EstadoNome = estado?.Nome
        };
    }
    public async Task<List<CidadeDto>> ObterCidadePorNome(string nome)
    {
        // Busca as cidades pelo nome
        var cidades = await _localidadeRepository.BuscarCidadesPorNomeAsync(nome);

        if (cidades == null || !cidades.Any())
        {
            throw new KeyNotFoundException($"Nenhuma cidade encontrada com o nome {nome}.");
        }

        var cidadesDto = new List<CidadeDto>();

        foreach (var cidade in cidades)
        {
            // Busca o estado associado à cidade
            var estado = await _localidadeRepository.ObterPorIdIntAsync(cidade.EstadoId);

            // Adiciona os dados da cidade e estado ao DTO
            cidadesDto.Add(new CidadeDto
            {
                Id = cidade.Id,
                Nome = cidade.Nome,
                EstadoId = cidade.EstadoId,
                EstadoNome = estado?.Nome
            });
        }

        return cidadesDto;
    }

    public async Task<BuscaCepDto> BuscarPorCepAsync(string cep)
    {
        // Verifica se o CEP é válido
        if (string.IsNullOrWhiteSpace(cep) || cep.Length != 8)
        {
            throw new ArgumentException("CEP inválido.");
        }

        // Consulta a API externa para buscar o CEP
        var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Erro ao buscar informações do CEP na API externa.");
        }

        var cepData = JsonSerializer.Deserialize<ViaCepDto>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (cepData == null || string.IsNullOrWhiteSpace(cepData.Localidade) || string.IsNullOrWhiteSpace(cepData.Uf))
        {
            throw new Exception("Informações do CEP não encontradas.");
        }

        // Busca cidade no banco pelo nome retornado pela API externa
        var cidade = await _localidadeRepository.BuscarCidadesPorNomeAsync(cepData.Localidade);
        var cidades = cidade.FirstOrDefault();

        if (cidade == null)
        {
            throw new KeyNotFoundException($"Cidade não encontrada para o nome: {cepData.Localidade}");
        }

        // Busca estado no banco pela UF retornada pela API externa
        var estado = await _localidadeRepository.BuscarEstadoPorSiglaAsync(cepData.Uf);
        if (estado == null)
        {
            throw new KeyNotFoundException($"Estado não encontrado para a UF: {cepData.Uf}");
        }

        // Retorna as informações em um DTO
        return new BuscaCepDto
        {
            Cep = cepData.Cep,
            Logradouro = cepData.Logradouro,
            Complemento = cepData.Complemento,
            Bairro = cepData.Bairro,
            CidadeId = cidades.Id,
            CidadeNome = cidades.Nome,
            EstadoId = estado.Id,
            EstadoNome = estado.Nome,
        };
    }

}