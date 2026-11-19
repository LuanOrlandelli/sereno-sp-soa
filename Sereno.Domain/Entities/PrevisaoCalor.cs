
using Sereno.Domain.Enums;

namespace Sereno.Domain.Entities;

public class PrevisaoCalor : EntidadeBase
{
    public DateOnly DataReferencia { get; private set; }

    public double Probabilidade { get; private set; }

    public NivelRisco Nivel { get; private set; }

    public string VersaoModelo { get; private set; } = string.Empty;

    public string Mensagem { get; private set; } = string.Empty;

    private PrevisaoCalor() { }

    public PrevisaoCalor(
        DateOnly dataReferencia,
        double probabilidade,
        NivelRisco nivel,
        string versaoModelo,
        string mensagem)
    {
        if (dataReferencia == default)
        {
            throw new ArgumentException(
                "A data de referência é obrigatória.");
        }

        if (!double.IsFinite(probabilidade) ||
            probabilidade < 0 ||
            probabilidade > 1)
        {
            throw new ArgumentOutOfRangeException(
                nameof(probabilidade),
                "A probabilidade deve estar entre 0 e 1.");
        }

        if (!Enum.IsDefined(nivel))
        {
            throw new ArgumentOutOfRangeException(
                nameof(nivel));
        }

        if (string.IsNullOrWhiteSpace(versaoModelo))
        {
            throw new ArgumentException(
                "A versão do modelo é obrigatória.");
        }

        if (string.IsNullOrWhiteSpace(mensagem))
        {
            throw new ArgumentException(
                "A mensagem é obrigatória.");
        }

        DataReferencia = dataReferencia;
        Probabilidade = probabilidade;
        Nivel = nivel;
        VersaoModelo = versaoModelo.Trim();
        Mensagem = mensagem.Trim();
    }
}
