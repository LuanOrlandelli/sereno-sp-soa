using System;
using System.Collections.Generic;
using System.Text;


using System.ComponentModel.DataAnnotations;

namespace Sereno.Application.DTOs;

public class CriarTotemDto
{
    [Required(ErrorMessage = "O código é obrigatório.")]
    [StringLength(50, MinimumLength = 3)]
    public string Codigo { get; set; } = string.Empty;

    public Guid? PontoRefrescoId { get; set; }
}

