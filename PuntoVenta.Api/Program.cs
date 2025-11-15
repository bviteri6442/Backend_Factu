using PuntoVenta.Infrastructure;
using PuntoVenta.Infrastructure.Persistencia;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MediatR;
using PuntoVenta.Api.Extensions;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Agregar Servicios de Infraestructura (DB, Identity, Repositorios)
builder.Services.AddInfrastructureServices(builder.Configuration);

// 2. Registrar MediatR
builder.Services.AddMediatR(typeof(Program).Assembly);

// Lectura de la clave secreta desde appsettings.json
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"] ?? "EstaEsUnaClaveSuperSecretaDeAlMenos32CaracteresParaFirmarJWT");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Registrar el documento Swagger "v1"
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PuntoVenta API",
        Version = "v1",
        Description = "API de ejemplo para PuntoVenta"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    // Servir Swagger UI en la raíz: https://localhost:56397/
    app.UseSwaggerUI(c =>
    {
        c.RoutePrefix = string.Empty; // hace que Swagger UI esté en "/"
        // Asegurar que Swagger UI apunte explícitamente al JSON generado
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PuntoVenta API v1");
    });
}

app.UseHttpsRedirection();

app.UseExceptionHandlingMiddleware();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Redirección opcional: si alguien abre "/", redirigir a "/swagger"
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();
