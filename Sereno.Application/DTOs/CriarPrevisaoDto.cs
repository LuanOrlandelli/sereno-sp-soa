
using System.ComponentModel.DataAnnotations;

namespace Sereno.Application.DTOs;

public class CriarPrevisaoDto
{
    [Required(
        ErrorMessage = "A data de referência é obrigatória.")]
    public DateOnly DataReferencia { get; set; }

    [Range(
        0.0,
        1.0,
        ErrorMessage = "A probabilidade deve estar entre 0 e 1.")]
    public double Probabilidade { get; set; }

    [Required(
        ErrorMessage = "A versão do modelo é obrigatória.")]
    [StringLength(100, MinimumLength = 1)]
    public string VersaoModelo { get; set; } = string.Empty;
}
