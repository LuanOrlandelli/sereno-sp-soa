
namespace Sereno.Application.DTOs;

public class AlertaRespostaDto
{
    public Guid Id { get; set; }

    public Guid LeituraSensorId { get; set; }

    public string TotemCodigo { get; set; } = string.Empty;

    public string Origem { get; set; } = string.Empty;

    public string Nivel { get; set; } = string.Empty;

    public string Mensagem { get; set; } = string.Empty;

    public bool Ativo { get; set; }

    public DateTime CriadoEm { get; set; }
}
