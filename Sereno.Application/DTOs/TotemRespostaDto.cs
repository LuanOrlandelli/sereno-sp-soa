using System;
using System.Collections.Generic;
using System.Text;


namespace Sereno.Application.DTOs;

public class TotemRespostaDto
{
    public Guid Id { get; set; }

    public string Codigo { get; set; } = string.Empty;

    public bool Ativo { get; set; }

    public Guid? PontoRefrescoId { get; set; }
}

