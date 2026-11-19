
using Sereno.Application.Services;
using Sereno.Domain.Enums;
using Xunit;

namespace Sereno.Tests.Services;

public class LeituraServiceTests
{
    // ==============================================
    // TESTES DE CLASSIFICACAO DE RISCO
    // ==============================================

    [Theory]
    [InlineData(20.0, NivelRisco.Normal)]
    [InlineData(31.99, NivelRisco.Normal)]
    [InlineData(32.0, NivelRisco.Atencao)]
    [InlineData(36.0, NivelRisco.Atencao)]
    [InlineData(40.99, NivelRisco.Atencao)]
    [InlineData(41.0, NivelRisco.Perigo)]
    [InlineData(50.0, NivelRisco.Perigo)]
    public void ClassificarRisco_DeveRetornarNivelCorreto(
        double indice,
        NivelRisco nivelEsperado)
    {
        var resultado = LeituraService.ClassificarRisco(
            indice);

        Assert.Equal(nivelEsperado, resultado);
    }

    // ==============================================
    // TESTE DO INDICE DE CALOR
    // ==============================================

    [Fact]
    public void CalcularIndiceCalor_CalorAlto_DeveGerarPerigo()
    {
        double indice = LeituraService.CalcularIndiceCalor(
            37.0,
            60.0);

        Assert.InRange(indice, 50.0, 55.0);

        var nivel = LeituraService.ClassificarRisco(
            indice);

        Assert.Equal(NivelRisco.Perigo, nivel);
    }

    [Fact]
    public void CalcularIndiceCalor_TemperaturaModerada_DeveSerFinito()
    {
        double indice = LeituraService.CalcularIndiceCalor(
            24.0,
            60.0);

        Assert.True(double.IsFinite(indice));

        Assert.InRange(indice, 20.0, 30.0);
    }

    // ==============================================
    // TESTES DE ENTRADAS INVALIDAS
    // ==============================================

    [Theory]
    [InlineData(-51.0, 60.0)]
    [InlineData(71.0, 60.0)]
    [InlineData(25.0, -1.0)]
    [InlineData(25.0, 101.0)]
    public void CalcularIndiceCalor_DadosInvalidos_DeveRejeitar(
        double temperatura,
        double umidade)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            LeituraService.CalcularIndiceCalor(
                temperatura,
                umidade));
    }

    [Fact]
    public void CalcularIndiceCalor_NaN_DeveRejeitar()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            LeituraService.CalcularIndiceCalor(
                double.NaN,
                60.0));
    }

    [Fact]
    public void CalcularIndiceCalor_Infinito_DeveRejeitar()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            LeituraService.CalcularIndiceCalor(
                30.0,
                double.PositiveInfinity));
    }
}
