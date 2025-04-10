using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DragonBallAPI.Core.Interfaces;
using DragonBallAPI.Infrastructure.Data;
using DragonBallAPI.Infrastructure.Data.Repositories;
using DragonBallAPI.Infrastructure.ExternalServices;
using DragonBallAPI.Application.Interfaces;
using DragonBallAPI.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using DragonBallAPI.Infrastructure.Seeders;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);


// Configurar servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar HttpClient para DragonBallApiService
builder.Services.AddHttpClient();

// Registrar Repositorios
builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();
builder.Services.AddScoped<ITransformationRepository, TransformationRepository>();

// Registrar Servicios
builder.Services.AddScoped<IExternalApiService, DragonBallApiService>();
builder.Services.AddScoped<ICharacterService, CharacterService>();
builder.Services.AddScoped<ITransformationService, TransformationService>();
builder.Services.AddScoped<ISyncService, SyncService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Configurar JWT Authentication
var secretKey = builder.Configuration["Jwt:Key"];
if (!string.IsNullOrEmpty(secretKey))
{
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });
}

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var seeder = new SeederFromApi(dbContext);
    await seeder.SeedAsync();
}

// Configure el pipeline de solicitudes HTTP

    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();

// Habilitar autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGet("/", () => Results.Redirect("/swagger"));


app.Run();