
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Sereno.Api.Controllers;
using Xunit;

namespace Sereno.Tests.Integration;

public class ApiHttpTests :
    IClassFixture<WebApplicationFactory<HealthController>>
{
    private readonly HttpClient _client;

    public ApiHttpTests(
        WebApplicationFactory<HealthController> factory)
    {
        _client = factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost")
            });
    }

    [Fact]
    public async Task BuscarTotem_ComIdInvalido_DeveRetornar404()
    {
        // Arrange: ID que não atende à restrição Guid da rota
        var url = "/api/totens/id-invalido";

        // Act
        var response = await _client.GetAsync(url);

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task CriarTotem_ComJsonInvalido_DeveRetornar400()
    {
        // Arrange: corpo JSON intencionalmente inválido
        using var content = new StringContent(
            "{ json invalido",
            System.Text.Encoding.UTF8,
            "application/json");

        // Act
        var response = await _client.PostAsync(
            "/api/totens",
            content);

        // Assert: valida o código HTTP
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);

        // Valida o corpo da resposta de erro
        var problema = await response.Content
            .ReadFromJsonAsync<
                Microsoft.AspNetCore.Mvc.ValidationProblemDetails>();

        Assert.NotNull(problema);
        Assert.Equal(400, problema.Status);
        Assert.NotEmpty(problema.Errors);
    }
}
