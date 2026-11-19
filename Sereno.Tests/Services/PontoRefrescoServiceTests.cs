
using Sereno.Application.Services;
using Xunit;

namespace Sereno.Tests.Services;

public class PontoRefrescoServiceTests
{
    // ==============================================
    // MESMO LOCAL = ZERO QUILOMETROS
    // ==============================================

    [Fact]
    public void CalcularDistancia_MesmoLocal_DeveRetornarZero()
    {
        double distancia =
            PontoRefrescoService.CalcularDistanciaKm(
                -23.505,
                -46.625,
                -23.505,
                -46.625);

        Assert.Equal(0.0, distancia);
    }

    // ==============================================
    // TESTAR DISTANCIA ENTRE PONTOS
    // ==============================================

    [Fact]
    public void CalcularDistancia_PontosDiferentes_DeveSerPositiva()
    {
        double distancia =
            PontoRefrescoService.CalcularDistanciaKm(
                -23.505,
                -46.625,
                -23.510,
                -46.630);

        Assert.True(distancia > 0);
        Assert.InRange(distancia, 0.7, 0.9);
    }

    // ==============================================
    // DISTANCIA SIMETRICA
    // ==============================================

    [Fact]
    public void CalcularDistancia_DeveSerSimetrica()
    {
        double ida =
            PontoRefrescoService.CalcularDistanciaKm(
                -23.505,
                -46.625,
                -23.520,
                -46.640);

        double volta =
            PontoRefrescoService.CalcularDistanciaKm(
                -23.520,
                -46.640,
                -23.505,
                -46.625);

        Assert.Equal(ida, volta);
    }

    // ==============================================
    // VALIDAR COORDENADAS
    // ==============================================

    [Theory]
    [InlineData(-91.0, -46.625)]
    [InlineData(91.0, -46.625)]
    [InlineData(-23.505, -181.0)]
    [InlineData(-23.505, 181.0)]
    public void CalcularDistancia_CoordenadasInvalidas_DeveRejeitar(
        double latitude,
        double longitude)
    {
        Assert.Throws<ArgumentException>(() =>
            PontoRefrescoService.CalcularDistanciaKm(
                latitude,
                longitude,
                -23.505,
                -46.625));
    }

    [Fact]
    public void CalcularDistancia_NaN_DeveRejeitar()
    {
        Assert.Throws<ArgumentException>(() =>
            PontoRefrescoService.CalcularDistanciaKm(
                double.NaN,
                -46.625,
                -23.505,
                -46.625));
    }
}
