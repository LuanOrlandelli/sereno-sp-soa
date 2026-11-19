
using Sereno.Domain.Entities;
using Sereno.Domain.Enums;
using Xunit;

namespace Sereno.Tests.Entities;

public class EntidadesTests
{
    // ==============================================
    // TOTEM
    // ==============================================

    [Fact]
    public void Totem_Valido_DeveSerCriadoAtivo()
    {
        var totem = new Totem("STN-TESTE");

        Assert.Equal("STN-TESTE", totem.Codigo);
        Assert.True(totem.Ativo);
        Assert.NotEqual(Guid.Empty, totem.Id);
    }

    [Fact]
    public void Totem_CodigoVazio_DeveRejeitar()
    {
        Assert.Throws<ArgumentException>(() =>
            new Totem(""));
    }

    [Fact]
    public void Totem_Desativar_DeveAlterarSituacao()
    {
        var totem = new Totem("STN-TESTE");

        totem.Desativar();

        Assert.False(totem.Ativo);

        totem.Ativar();

        Assert.True(totem.Ativo);
    }

    // ==============================================
    // PONTO DE REFRESCO
    // ==============================================

    [Fact]
    public void PontoRefresco_Valido_DeveSerCriado()
    {
        var ponto = new PontoRefresco(
            "Ponto Teste",
            "Endereco ficticio em Santana",
            -23.505,
            -46.625,
            true,
            false);

        Assert.Equal("Ponto Teste", ponto.Nome);
        Assert.True(ponto.TemAgua);
        Assert.False(ponto.TemSombra);
    }

    [Fact]
    public void PontoRefresco_SemAguaESombra_DeveRejeitar()
    {
        Assert.Throws<ArgumentException>(() =>
            new PontoRefresco(
                "Ponto Teste",
                "Endereco ficticio em Santana",
                -23.505,
                -46.625,
                false,
                false));
    }

    [Fact]
    public void PontoRefresco_Atualizar_DeveModificarDados()
    {
        var ponto = new PontoRefresco(
            "Ponto Original",
            "Endereco original de teste",
            -23.505,
            -46.625,
            true,
            false);

        ponto.Atualizar(
            "Ponto Atualizado",
            "Endereco atualizado de teste",
            -23.510,
            -46.630,
            true,
            true);

        Assert.Equal("Ponto Atualizado", ponto.Nome);
        Assert.True(ponto.TemAgua);
        Assert.True(ponto.TemSombra);
    }

    // ==============================================
    // PREVISAO DE CALOR
    // ==============================================

    [Fact]
    public void PrevisaoCalor_Valida_DeveSerCriada()
    {
        var previsao = new PrevisaoCalor(
            new DateOnly(2026, 11, 18),
            0.82,
            NivelRisco.Perigo,
            "modelo-teste",
            "Calor perigoso previsto.");

        Assert.Equal(0.82, previsao.Probabilidade);
        Assert.Equal(NivelRisco.Perigo, previsao.Nivel);
    }

    [Fact]
    public void PrevisaoCalor_ProbabilidadeInvalida_DeveRejeitar()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new PrevisaoCalor(
                new DateOnly(2026, 11, 18),
                1.5,
                NivelRisco.Perigo,
                "modelo-teste",
                "Mensagem de teste"));
    }
}
