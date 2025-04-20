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
using LudusApp.Application.Dtos;
using LudusApp.Application.DTOs;
using LudusApp.Application.Dtos.Local;
using LudusApp.Application.Interfaces.ReadOnly.Localidade;
using LudusApp.Application.Mapper;
using LudusApp.Domain.Entities.Evento;
using LudusApp.Domain.Entities.Local;
using LudusApp.Domain.TemaSettings;
using LudusApp.Domain.Services;
using LudusApp.Domain.Interfaces.Email;
using LudusApp.Domain.Interfaces.Evento;
using LudusApp.Domain.Interfaces.Local;
using LudusApp.Domain.Interfaces.TemaSettings;
using LudusApp.Infra.Data.Repositories.Email;
using LudusApp.Infra.Data.Repositories.Evento;
using LudusApp.Infra.Data.Repositories.Local;
using LudusApp.Infra.Data.Repositories.ReadOnly.Localidade;
using LudusApp.Infra.Data.Repositories.TemasSettings;

public static class ServiceConfiguration
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = Environment.GetEnvironmentVariable("LUDUSAPP_DB")
                               ?? configuration.GetConnectionString("LudusDevelopment");
        // var connectionString = configuration.GetConnectionString("LudusAppContext");

        services.AddHangfire(config => { config.UsePostgreSqlStorage(connectionString); });
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
        services.AddScoped<ILocalidadeReadRepository, LocalidadeReadRepository>();
        services.AddHostedService<SincronizacaoBackgroundService>();

        services.AddSingleton<IAuthorizationHandler, IdadeAuthorization>();
        services.AddScoped<IAuthorizationHandler, EmpresaRequirementHandler>();

        services.AddScoped<IUsuarioEmpresaRepository, UsuarioEmpresaRespository>();

        services.AddScoped<ITemaRepository, TemaRepository>();
        services.AddScoped<IMapper<Tema, TemaReadDto, TemaReadDto, TemaReadDto>, TemaMapper>();
        services.AddScoped<IRepositoryBase<Tema>, TemaRepository>();
        services.AddScoped<TemaService>();

        services.AddScoped<ILocalRepository, LocalRepository>();
        services.AddScoped<IMapper<Local, LocalReadDto, LocalCreateDto, LocalUpdateDto>, LocalMapper>();
        services.AddScoped<LocalService>();

        services.AddScoped<IEventoRespository, EventoRepository>();
        services.AddScoped<IMapper<Evento, EventoReadDto, EventoCreateDto, EventoUpdateDto>, EventoMapper>();
        services.AddScoped<EventoService>();


        return services;
    }
}