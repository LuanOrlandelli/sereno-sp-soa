
using Microsoft.EntityFrameworkCore;
using Sereno.Application.Interfaces;
using Sereno.Application.Services;
using Sereno.Infrastructure.Data;
using Sereno.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ==================================================
// 1. CONFIGURAÇÃO DO BANCO DE DADOS
// ==================================================

builder.Services.AddDbContext<SerenoDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString(
            "DefaultConnection")));

// ==================================================
// 2. INJEÇÃO DE DEPENDÊNCIA
// ==================================================

// Repositórios

builder.Services.AddScoped<
    ITotemRepository,
    TotemRepository>();

builder.Services.AddScoped<
    ILeituraRepository,
    LeituraRepository>();

builder.Services.AddScoped<
    IAlertaRepository,
    AlertaRepository>();

builder.Services.AddScoped<
    IPontoRefrescoRepository,
    PontoRefrescoRepository>();

builder.Services.AddScoped<
    IPrevisaoRepository,
    PrevisaoRepository>();

// Serviços

builder.Services.AddScoped<TotemService>();

builder.Services.AddScoped<LeituraService>();

builder.Services.AddScoped<AlertaService>();

builder.Services.AddScoped<PontoRefrescoService>();

builder.Services.AddScoped<PrevisaoService>();

// ==================================================
// 3. CONFIGURAÇÃO DA API
// ==================================================

builder.Services.AddControllers();

// Geração automática da documentação OpenAPI
builder.Services.AddOpenApi();

// Serviços de autorização
builder.Services.AddAuthorization();

// ==================================================
// 4. CONSTRUÇÃO DA APLICAÇÃO
// ==================================================

var app = builder.Build();

// ==================================================
// 5. DOCUMENTAÇÃO OPENAPI E SWAGGER
// ==================================================

if (app.Environment.IsDevelopment())
{
    // Documento OpenAPI em formato JSON
    app.MapOpenApi();

    // Interface interativa Swagger UI
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "Sereno SP API v1");

        options.DocumentTitle =
            "Sereno SP - Documentação da API";

        options.RoutePrefix = "swagger";
    });
}

// ==================================================
// 6. PIPELINE HTTP
// ==================================================

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// ==================================================
// 7. INICIAR A APLICAÇÃO
// ==================================================

app.Run();
