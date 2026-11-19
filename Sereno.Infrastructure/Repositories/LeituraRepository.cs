
using Microsoft.EntityFrameworkCore;
using Sereno.Application.Interfaces;
using Sereno.Domain.Entities;
using Sereno.Infrastructure.Data;

namespace Sereno.Infrastructure.Repositories;

public class LeituraRepository : ILeituraRepository
{
    private readonly SerenoDbContext _context;

    public LeituraRepository(SerenoDbContext context)
    {
        _context = context;
    }

    public async Task<Totem?> BuscarTotemPorCodigoAsync(
        string codigo,
        CancellationToken cancellationToken = default)
    {
        return await _context.Totens
            .AsNoTracking()
            .FirstOrDefaultAsync(
                t => t.Codigo == codigo,
                cancellationToken);
    }

    public async Task<LeituraSensor> AdicionarAsync(
        LeituraSensor leitura,
        Alerta? alerta,
        CancellationToken cancellationToken = default)
    {
        await _context.Leituras.AddAsync(
            leitura,
            cancellationToken);

        if (alerta is not null)
        {
            await _context.Alertas.AddAsync(
                alerta,
                cancellationToken);
        }

        await _context.SaveChangesAsync(
            cancellationToken);

        return leitura;
    }

    public async Task<LeituraSensor?> BuscarUltimaAsync(
        string codigoTotem,
        CancellationToken cancellationToken = default)
    {
        return await _context.Leituras
            .AsNoTracking()
            .Where(l => l.Totem.Codigo == codigoTotem)
            .OrderByDescending(l => l.ColetadaEm)
            .ThenByDescending(l => l.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
