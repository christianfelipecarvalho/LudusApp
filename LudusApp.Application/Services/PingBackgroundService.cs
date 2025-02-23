using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LudusApp.Application.Services;

public class PingBackgroundService : BackgroundService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<PingBackgroundService> _logger;

    public PingBackgroundService(HttpClient httpClient, ILogger<PingBackgroundService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Faz a requisição para manter o servidor ativo
                var response = await _httpClient.GetAsync("http://localhost/api/ping", stoppingToken);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Ping realizado com sucesso. -> {DateTime.Now}");
                }
                else
                {
                    _logger.LogWarning($"Falha no ping: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao realizar ping: {ex.Message}");
            }

            // Aguarda 1 minutos antes do próximo ping
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
