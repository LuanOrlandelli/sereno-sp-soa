
namespace Sereno.Application.DTOs;

public class PrevisaoRespostaDto
{
    public Guid Id { get; set; }

    public DateOnly DataReferencia { get; set; }

    public double Probabilidade { get; set; }

    public double ProbabilidadePercentual { get; set; }

    public string Nivel { get; set; } = string.Empty;

    public string Mensagem { get; set; } = string.Empty;

    public string VersaoModelo { get; set; } = string.Empty;

    public DateTime CriadoEm { get; set; }
}
