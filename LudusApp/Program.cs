using System.Text;
using LudusApp.Application.Authorization;
using LudusApp.Application.Services;
using LudusApp.Domain.Usuarios;
using LudusApp.Infra.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://0.0.0.0:80");
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromDays(365); // Adicionando para não encerrar a applicacao
});

builder.Services.AddHttpClient(); // adicionado para o servico de ping
builder.Services.AddHostedService<PingBackgroundService>();

var connectionString = Environment.GetEnvironmentVariable("LUDUSAPP_DB")
?? builder.Configuration.GetConnectionString("LudusAppContext");

Console.WriteLine($"Connection String Usada: {(connectionString != null ? "Definida" : "Não definida")}");

builder.Services.AddDbContext<LudusAppContext>(opts => opts.UseNpgsql(connectionString));
// Add services to the container.
builder.Services.AddIdentity<Usuario, IdentityRole>().AddEntityFrameworkStores<LudusAppContext>().AddDefaultTokenProviders();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<UsuarioService>();
//builder.Services.AddScoped<TokenService>(); -> Forma Consome menos memoria
builder.Services.AddSingleton<TokenService>(); // -> Forma mais rapida

builder.Services.AddSingleton<IAuthorizationHandler, IdadeAuthorization>();

builder.Services.AddControllers().AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    // Adiciona o esquema de segurança para autenticação com JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    // Adiciona o requisito de segurança globalmente, indicando que todos os endpoints requerem autenticação
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

///Quando for adicionar o issuer e audience habilitar essas variaveis para buscar do appsettings.
//var jwtSettings = builder.Configuration.GetSection("Jwt");
//var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("6A4FSD568DS1F652VDV1FD656ED4FDds456ds13")),
        ValidateAudience = false,
        ValidateIssuer = false,
        ClockSkew = TimeSpan.Zero
    };
    /// colocar aqui para pegar o issuer e audience da variavel de ambiente ou appsettings para garantir a segurança do token---
    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync("{\"error\": \"Não autorizado. Token ausente ou inválido.\"}");
        }
    };
});
builder.Services.AddAuthentication()
    .AddGoogle(googleOptions =>
    {
        // Tenta pegar primeiro das variáveis de ambiente, se não encontrar, pega do appsettings
        googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"] ??
                                 Environment.GetEnvironmentVariable("Authentication_Google_ClientId");

        googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ??
                                     Environment.GetEnvironmentVariable("Authentication_Google_ClientSecret");
    });


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IdadeMinima", policcy => policcy.AddRequirements(new IdadeMinima(18)));
});

var app = builder.Build();

// Aqui verificar se crio a variavel agora ou mais pra frente...
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
