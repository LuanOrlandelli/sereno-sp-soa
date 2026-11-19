
using Microsoft.EntityFrameworkCore;
using Sereno.Application.Interfaces;
using Sereno.Domain.Entities;
using Sereno.Infrastructure.Data;

namespace Sereno.Infrastructure.Repositories;

public class AlertaRepository : IAlertaRepository
{
    private readonly SerenoDbContext _context;

    public AlertaRepository(SerenoDbContext context)
    {
        _context = context;
    }

    public async Task<List<Alerta>> ListarAsync(
        bool? ativos = true,
        string? totemCodigo = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Alerta> query = _context.Alertas
            .AsNoTracking();

        if (ativos.HasValue)
        {
            query = query.Where(
                a => a.Ativo == ativos.Value);
        }

        if (!string.IsNullOrWhiteSpace(totemCodigo))
        {
            var codigo = totemCodigo.Trim();

            query = query.Where(
                a => a.TotemCodigo == codigo);
        }

        return await query
            .OrderByDescending(a => a.CriadoEm)
            .ThenByDescending(a => a.Id)
            .ToListAsync(cancellationToken);
    }
}
