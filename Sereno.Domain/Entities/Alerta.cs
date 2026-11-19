
using Sereno.Domain.Enums;

namespace Sereno.Domain.Entities;

public class Alerta : EntidadeBase
{
    public Guid LeituraSensorId { get; private set; }

    public string TotemCodigo { get; private set; } = string.Empty;

    public string Origem { get; private set; } = string.Empty;

    public NivelRisco Nivel { get; private set; }

    public string Mensagem { get; private set; } = string.Empty;

    public bool Ativo { get; private set; } = true;

    private Alerta() { }

    public Alerta(
        Guid leituraSensorId,
        string totemCodigo,
        NivelRisco nivel,
        string mensagem)
    {
        if (leituraSensorId == Guid.Empty)
            throw new ArgumentException(
                "A leitura do alerta é obrigatória.");

        if (string.IsNullOrWhiteSpace(totemCodigo))
            throw new ArgumentException(
                "O código do totem é obrigatório.");

        if (string.IsNullOrWhiteSpace(mensagem))
            throw new ArgumentException(
                "A mensagem do alerta é obrigatória.");

        LeituraSensorId = leituraSensorId;
        TotemCodigo = totemCodigo.Trim();
        Origem = "Totem";
        Nivel = nivel;
        Mensagem = mensagem.Trim();
        Ativo = true;
    }

    public void Desativar()
    {
        Ativo = false;
    }
}
