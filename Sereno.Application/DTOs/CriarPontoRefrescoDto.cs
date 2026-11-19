
using System.ComponentModel.DataAnnotations;

namespace Sereno.Application.DTOs;

public class CriarPontoRefrescoDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(150, MinimumLength = 3)]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O endereço é obrigatório.")]
    [StringLength(250, MinimumLength = 5)]
    public string Endereco { get; set; } = string.Empty;

    [Range(-90, 90, ErrorMessage = "Latitude inválida.")]
    public double Latitude { get; set; }

    [Range(-180, 180, ErrorMessage = "Longitude inválida.")]
    public double Longitude { get; set; }

    public bool TemAgua { get; set; }

    public bool TemSombra { get; set; }
}
