
using Sereno.Application.DTOs;
using Sereno.Application.Interfaces;
using Sereno.Domain.Entities;
using Sereno.Domain.Enums;

namespace Sereno.Application.Services;

public class PrevisaoService
{
    private readonly IPrevisaoRepository _repository;

    public PrevisaoService(IPrevisaoRepository repository)
    {
        _repository = repository;
    }

    // ==================================================
    // CADASTRAR UMA NOVA PREVISAO
    // ==================================================

    public async Task<PrevisaoRespostaDto> CriarAsync(
        CriarPrevisaoDto dto,
        CancellationToken cancellationToken = default)
    {
        if (dto.DataReferencia == default)
        {
            throw new ArgumentException(
                "A data de referência é obrigatória.");
        }

        if (!double.IsFinite(dto.Probabilidade) ||
            dto.Probabilidade < 0 ||
            dto.Probabilidade > 1)
        {
            throw new ArgumentException(
                "A probabilidade deve estar entre 0 e 1.");
        }

        if (string.IsNullOrWhiteSpace(dto.VersaoModelo))
        {
            throw new ArgumentException(
                "A versão do modelo é obrigatória.");
        }

        if (dto.VersaoModelo.Trim().Length > 100)
        {
            throw new ArgumentException(
                "A versão do modelo deve ter até 100 caracteres.");
        }

        bool existe = await _repository.ExisteParaDataAsync(
            dto.DataReferencia,
            cancellationToken);

        if (existe)
        {
            throw new InvalidOperationException(
                "Já existe uma previsão para essa data.");
        }

        NivelRisco nivel = ClassificarProbabilidade(
            dto.Probabilidade);

        string mensagem = GerarMensagem(nivel);

        var previsao = new PrevisaoCalor(
            dto.DataReferencia,
            dto.Probabilidade,
            nivel,
            dto.VersaoModelo,
            mensagem);

        var salva = await _repository.AdicionarAsync(
            previsao,
            cancellationToken);

        return ConverterParaDto(salva);
    }

    // ==================================================
    // CONSULTAR PREVISAO DE AMANHA
    // ==================================================

    public async Task<PrevisaoRespostaDto?> BuscarAmanhaAsync(
        CancellationToken cancellationToken = default)
    {
        var fuso = TimeZoneInfo.FindSystemTimeZoneById(
            "America/Sao_Paulo");

        DateTime agoraLocal = TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.UtcNow,
            fuso);

        DateOnly amanha = DateOnly.FromDateTime(
            agoraLocal.AddDays(1));

        return await BuscarPorDataAsync(
            amanha,
            cancellationToken);
    }

    // ==================================================
    // CONSULTAR PREVISAO POR DATA
    // ==================================================

    public async Task<PrevisaoRespostaDto?> BuscarPorDataAsync(
        DateOnly dataReferencia,
        CancellationToken cancellationToken = default)
    {
        if (dataReferencia == default)
        {
            throw new ArgumentException(
                "A data de referência é obrigatória.");
        }

        var previsao = await _repository.BuscarPorDataAsync(
            dataReferencia,
            cancellationToken);

        if (previsao is null)
            return null;

        return ConverterParaDto(previsao);
    }

    // ==================================================
    // CLASSIFICAR A PROBABILIDADE
    // ==================================================

    public static NivelRisco ClassificarProbabilidade(
        double probabilidade)
    {
        if (!double.IsFinite(probabilidade) ||
            probabilidade < 0 ||
            probabilidade > 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(probabilidade));
        }

        if (probabilidade > 0.70)
            return NivelRisco.Perigo;

        if (probabilidade >= 0.40)
            return NivelRisco.Atencao;

        return NivelRisco.Normal;
    }

    // ==================================================
    // GERAR MENSAGEM PARA O CIDADAO
    // ==================================================

    public static string GerarMensagem(NivelRisco nivel)
    {
        return nivel switch
        {
            NivelRisco.Perigo =>
                "Amanhã: calor perigoso. " +
                "Procure se proteger e evite exposição prolongada ao sol.",

            NivelRisco.Atencao =>
                "Amanhã pode ser muito quente. " +
                "Prepare-se e mantenha água por perto.",

            NivelRisco.Normal =>
                "Sem alerta antecipado de calor extremo " +
                "para amanhã.",

            _ => throw new ArgumentOutOfRangeException(
                nameof(nivel))
        };
    }

    // ==================================================
    // CONVERTER ENTIDADE PARA DTO
    // ==================================================

    private static PrevisaoRespostaDto ConverterParaDto(
        PrevisaoCalor previsao)
    {
        return new PrevisaoRespostaDto
        {
            Id = previsao.Id,
            DataReferencia = previsao.DataReferencia,
            Probabilidade = previsao.Probabilidade,
            ProbabilidadePercentual = Math.Round(
                previsao.Probabilidade * 100,
                2),
            Nivel = previsao.Nivel.ToString(),
            Mensagem = previsao.Mensagem,
            VersaoModelo = previsao.VersaoModelo,
            CriadoEm = previsao.CriadoEm
        };
    }
}
