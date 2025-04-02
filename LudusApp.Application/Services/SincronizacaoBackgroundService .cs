using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LudusApp.Application.Services;

public class SincronizacaoBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public SincronizacaoBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var atrasoInicial = TimeSpan.FromDays(20);
        await Task.Delay(atrasoInicial, stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var localidadeService = scope.ServiceProvider.GetRequiredService<LocalidadeService>();
                await localidadeService.SincronizarLocalidadesAsync();
            }

            // Aguarda 1 mês
            await Task.Delay(TimeSpan.FromDays(30), stoppingToken);
        }
    }
}