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
    serverOptions.Limits.KeepAliveTimeout = TimeSpan.FromDays(365); // Adicionando para n�o encerrar a applicacao
});

builder.Services.AddHttpClient(); // adicionado para o servico de ping
builder.Services.AddHostedService<PingBackgroundService>();

var connectionString = Environment.GetEnvironmentVariable("LUDUSAPP_DB")
?? builder.Configuration.GetConnectionString("LudusAppContext");

Console.WriteLine($"Connection String Usada: {(connectionString != null ? "Definida" : "N�o definida")}");

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
    // Adiciona o esquema de seguran�a para autentica��o com JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    // Adiciona o requisito de seguran�a globalmente, indicando que todos os endpoints requerem autentica��o
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
    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync("{\"error\": \"N�o autorizado. Token ausente ou inv�lido.\"}");
        }
    };
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

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
