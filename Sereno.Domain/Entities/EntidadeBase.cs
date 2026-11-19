using System;
using System.Collections.Generic;
using System.Text;

namespace Sereno.Domain.Entities;

public abstract class EntidadeBase
{
    public Guid Id { get; protected set; } = Guid.NewGuid();

    public DateTime CriadoEm { get; protected set; } = DateTime.UtcNow;
}