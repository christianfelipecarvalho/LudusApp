using Hangfire;
using LudusApp.Application.Services;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Sinks.Http;


var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:80")
    .ConfigureKestrel(serverOptions =>
    {
        serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromDays(365);
    });

builder.Services
    .AddHttpClient()
    .ConfigureServices(builder.Configuration)
    .ConfigureSwagger()
    .ConfigureAuthentication(builder.Configuration)
    .ConfigureAuthorization();

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();



app.UseHangfireDashboard();
RecurringJob.AddOrUpdate<LocalidadeService>(
    "SincronizarLocalidades",
    service => service.SincronizarLocalidadesAsync(),
    Cron.Monthly);

var logger = new LoggerConfiguration()
    .WriteTo.File(
        path: "logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        formatter: new CompactJsonFormatter()) 
    .CreateLogger();

logger.Information("Aplicação iniciada com sucesso!");
logger.Error("Erro ao processar requisição.");



app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();