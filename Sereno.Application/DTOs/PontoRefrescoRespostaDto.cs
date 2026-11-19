
namespace Sereno.Application.DTOs;

public class PontoRefrescoRespostaDto
{
    public Guid Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Endereco { get; set; } = string.Empty;

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public bool TemAgua { get; set; }

    public bool TemSombra { get; set; }

    public double? DistanciaKm { get; set; }
}
