
using Sereno.Application.Services;
using Sereno.Domain.Enums;
using Xunit;

namespace Sereno.Tests.Services;

public class PrevisaoServiceTests
{
    // ==============================================
    // TESTAR CLASSIFICACAO DE PROBABILIDADE
    // ==============================================

    [Theory]
    [InlineData(0.0, NivelRisco.Normal)]
    [InlineData(0.20, NivelRisco.Normal)]
    [InlineData(0.39, NivelRisco.Normal)]
    [InlineData(0.40, NivelRisco.Atencao)]
    [InlineData(0.60, NivelRisco.Atencao)]
    [InlineData(0.70, NivelRisco.Atencao)]
    [InlineData(0.71, NivelRisco.Perigo)]
    [InlineData(0.82, NivelRisco.Perigo)]
    [InlineData(1.0, NivelRisco.Perigo)]
    public void ClassificarProbabilidade_DeveRetornarNivelCorreto(
        double probabilidade,
        NivelRisco nivelEsperado)
    {
        var resultado =
            PrevisaoService.ClassificarProbabilidade(
                probabilidade);

        Assert.Equal(nivelEsperado, resultado);
    }

    // ==============================================
    // TESTAR PROBABILIDADES INVALIDAS
    // ==============================================

    [Theory]
    [InlineData(-0.01)]
    [InlineData(1.01)]
    [InlineData(2.0)]
    public void ClassificarProbabilidade_Invalida_DeveRejeitar(
        double probabilidade)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            PrevisaoService.ClassificarProbabilidade(
                probabilidade));
    }

    [Fact]
    public void ClassificarProbabilidade_NaN_DeveRejeitar()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            PrevisaoService.ClassificarProbabilidade(
                double.NaN));
    }

    // ==============================================
    // TESTAR MENSAGENS GERADAS
    // ==============================================

    [Theory]
    [InlineData(NivelRisco.Normal)]
    [InlineData(NivelRisco.Atencao)]
    [InlineData(NivelRisco.Perigo)]
    public void GerarMensagem_DeveRetornarMensagem(
        NivelRisco nivel)
    {
        string mensagem =
            PrevisaoService.GerarMensagem(nivel);

        Assert.False(string.IsNullOrWhiteSpace(mensagem));
    }

    [Fact]
    public void GerarMensagem_Perigo_DeveOrientarProtecao()
    {
        string mensagem = PrevisaoService.GerarMensagem(
            NivelRisco.Perigo);

        Assert.Contains("calor perigoso", mensagem);
    }

    [Fact]
    public void GerarMensagem_NivelInvalido_DeveRejeitar()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            PrevisaoService.GerarMensagem(
                (NivelRisco)999));
    }
}
