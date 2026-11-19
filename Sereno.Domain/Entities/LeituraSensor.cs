using System;
using System.Collections.Generic;
using System.Text;


using Sereno.Domain.Enums;

namespace Sereno.Domain.Entities;

public class LeituraSensor : EntidadeBase
{
    public Guid TotemId { get; private set; }

    public Totem Totem { get; private set; } = null!;

    public double Temperatura { get; private set; }

    public double Umidade { get; private set; }

    public double IndiceCalor { get; private set; }

    public NivelRisco Nivel { get; private set; }

    public DateTime ColetadaEm { get; private set; }

    private LeituraSensor() { }

    public LeituraSensor(
        Guid totemId,
        double temperatura,
        double umidade,
        double indiceCalor,
        NivelRisco nivel,
        DateTime coletadaEm)
    {
        if (totemId == Guid.Empty)
            throw new ArgumentException("Totem inválido.");

        if (umidade < 0 || umidade > 100)
            throw new ArgumentOutOfRangeException(nameof(umidade));

        if (temperatura < -50 || temperatura > 70)
            throw new ArgumentOutOfRangeException(nameof(temperatura));

        if (double.IsNaN(indiceCalor) ||
            double.IsInfinity(indiceCalor))
            throw new ArgumentException("Índice de calor inválido.");

        if (!Enum.IsDefined(nivel))
            throw new ArgumentOutOfRangeException(nameof(nivel));

        TotemId = totemId;
        Temperatura = temperatura;
        Umidade = umidade;
        IndiceCalor = indiceCalor;
        Nivel = nivel;
        ColetadaEm = coletadaEm.ToUniversalTime();
    }
}
