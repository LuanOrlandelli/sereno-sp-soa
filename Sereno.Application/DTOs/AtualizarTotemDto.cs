
using System.ComponentModel.DataAnnotations;

namespace Sereno.Application.DTOs;

public class AtualizarTotemDto
{
    [Required(ErrorMessage = "O código é obrigatório.")]
    [StringLength(50, MinimumLength = 3)]
    public string Codigo { get; set; } = string.Empty;

    public Guid? PontoRefrescoId { get; set; }

    public bool Ativo { get; set; }
}
