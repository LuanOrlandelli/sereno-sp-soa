
using Microsoft.EntityFrameworkCore;
using Sereno.Application.Interfaces;
using Sereno.Domain.Entities;
using Sereno.Infrastructure.Data;

namespace Sereno.Infrastructure.Repositories;

public class PontoRefrescoRepository : IPontoRefrescoRepository
{
    private readonly SerenoDbContext _context;

    public PontoRefrescoRepository(
        SerenoDbContext context)
    {
        _context = context;
    }

    // ==================================================
    // CREATE - CADASTRAR
    // ==================================================

    public async Task<PontoRefresco> AdicionarAsync(
        PontoRefresco ponto,
        CancellationToken cancellationToken = default)
    {
        await _context.PontosRefresco.AddAsync(
            ponto,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);

        return ponto;
    }

    // ==================================================
    // READ - LISTAR COM FILTROS
    // ==================================================

    public async Task<List<PontoRefresco>> ListarAsync(
        bool? temAgua = null,
        bool? temSombra = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<PontoRefresco> query =
            _context.PontosRefresco.AsNoTracking();

        if (temAgua.HasValue)
        {
            query = query.Where(
                p => p.TemAgua == temAgua.Value);
        }

        if (temSombra.HasValue)
        {
            query = query.Where(
                p => p.TemSombra == temSombra.Value);
        }

        return await query
            .OrderBy(p => p.Nome)
            .ToListAsync(cancellationToken);
    }

    // ==================================================
    // READ - BUSCAR POR ID
    // ==================================================

    public async Task<PontoRefresco?> BuscarPorIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.PontosRefresco
            .FirstOrDefaultAsync(
                p => p.Id == id,
                cancellationToken);
    }

    // ==================================================
    // UPDATE - ATUALIZAR
    // ==================================================

    public async Task<PontoRefresco> AtualizarAsync(
        PontoRefresco ponto,
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(
            cancellationToken);

        return ponto;
    }

    // ==================================================
    // VERIFICAR VINCULO COM TOTENS
    // ==================================================

    public async Task<bool> PossuiTotensAsync(
        Guid pontoId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Totens
            .AnyAsync(
                t => t.PontoRefrescoId == pontoId,
                cancellationToken);
    }

    // ==================================================
    // DELETE - EXCLUIR
    // ==================================================

    public async Task ExcluirAsync(
        PontoRefresco ponto,
        CancellationToken cancellationToken = default)
    {
        _context.PontosRefresco.Remove(ponto);

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}
