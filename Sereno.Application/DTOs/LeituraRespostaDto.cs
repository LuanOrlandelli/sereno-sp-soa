using System;
using System.Collections.Generic;
using System.Text;


namespace Sereno.Application.DTOs;

public class LeituraRespostaDto
{
    public Guid Id { get; set; }

    public string TotemCodigo { get; set; } = string.Empty;

    public double Temperatura { get; set; }

    public double Umidade { get; set; }

    public double IndiceCalor { get; set; }

    public string Nivel { get; set; } = string.Empty;

    public DateTime ColetadaEm { get; set; }
}
