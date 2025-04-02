using LudusApp.Application.Authorization;
using LudusApp.Application.Services;
using LudusApp.Domain.Interfaces.Base;
using LudusApp.Domain.Interfaces;
using LudusApp.Domain.Usuarios;
using LudusApp.Infra.Data.Repositories.Base;
using LudusApp.Infra.Data.Repositories;
using LudusApp.Infra.Data;
using LudusApp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Hangfire.PostgreSql;
using LudusApp.Domain.Interfaces.VinculoUsuarioEmpresa;
using System.Data;
using LudusApp.Domain.TemaSettings;
using LudusApp.Infra.Data.Repositories.TemasSettings;
using LudusApp.Domain.Services;
using LudusApp.Domain.Interfaces.Email;
using LudusApp.Infra.Data.Repositories.Email;

public static class ServiceConfiguration
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = Environment.GetEnvironmentVariable("LUDUSAPP_DB")
            ?? configuration.GetConnectionString("LudusDevelopment");

        services.AddHangfire(config =>
        {
            config.UsePostgreSqlStorage(connectionString); 
        });
        services.AddHangfireServer();

        Console.WriteLine($"Connection String Usada: {(connectionString != null ? "Definida" : "Não definida")}");

        // Configuração do Entity Framework
        services.AddDbContext<LudusAppContext>(opts => opts.UseNpgsql(connectionString));
        services.AddIdentity<Usuario, IdentityRole>()
            .AddEntityFrameworkStores<LudusAppContext>()
            .AddDefaultTokenProviders();

        // Configuração para Dapper (reutilizando a conexão do EF)
        services.AddTransient<IDbConnection>(provider =>
            provider.GetRequiredService<LudusAppContext>().Database.GetDbConnection());

        //Outros serviços
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddScoped<UsuarioService>();
        services.AddScoped<UsuarioDomainService>();
        services.AddSingleton<TokenService>();

        services.AddScoped<IEmailRespository, EmailRepository>();
        services.AddScoped<ITemplateEmailRepository, TemplateEmailRepository>();
        services.AddScoped<IConfiguracaoEmailRepository, ConfiguracaoEmailRepository>();
        services.AddScoped<EmailService>();

        services.AddScoped<IEmpresaRepository, EmpresaRepository>();
        services.AddScoped<EmpresaService>();

        services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
        services.AddScoped<ILocalidadeRepository, LocalidadeRepository>();
        services.AddHttpClient<LocalidadeService>();
        services.AddHostedService<SincronizacaoBackgroundService>();
        services.AddSingleton<IAuthorizationHandler, IdadeAuthorization>();
        services.AddScoped<IUsuarioEmpresaRepository, UsuarioEmpresaRespository>();
        services.AddScoped<IRepositoryBase<Tema>, TemaRepository>();
        services.AddScoped<TemaService>();



        return services;
    }
}