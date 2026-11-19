using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Sereno.Application.DTOs;

public class CriarLeituraDto
{
    [Required(ErrorMessage = "O código do totem é obrigatório.")]
    public string TotemCodigo { get; set; } = string.Empty;

    [Range(-50, 70, ErrorMessage = "Temperatura fora da faixa permitida.")]
    public double Temperatura { get; set; }

    [Range(0, 100, ErrorMessage = "Umidade deve estar entre 0 e 100.")]
    public double Umidade { get; set; }
}
