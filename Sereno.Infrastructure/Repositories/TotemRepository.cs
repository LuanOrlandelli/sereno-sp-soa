
using Microsoft.EntityFrameworkCore;
using Sereno.Application.Interfaces;
using Sereno.Domain.Entities;
using Sereno.Infrastructure.Data;

namespace Sereno.Infrastructure.Repositories;

public class TotemRepository : ITotemRepository
{
    private readonly SerenoDbContext _context;

    public TotemRepository(SerenoDbContext context)
    {
        _context = context;
    }

    // ==================================================
    // VERIFICAR CODIGO EXISTENTE
    // ==================================================

    public async Task<bool> ExisteCodigoAsync(
        string codigo,
        CancellationToken cancellationToken = default)
    {
        return await _context.Totens
            .AnyAsync(
                t => t.Codigo == codigo,
                cancellationToken);
    }

    // ==================================================
    // VERIFICAR CODIGO EM OUTRO TOTEM
    // ==================================================

    public async Task<bool> ExisteOutroCodigoAsync(
        string codigo,
        Guid idAtual,
        CancellationToken cancellationToken = default)
    {
        return await _context.Totens
            .AnyAsync(
                t => t.Codigo == codigo &&
                     t.Id != idAtual,
                cancellationToken);
    }

    // ==================================================
    // BUSCAR PONTO DE REFRESCO
    // ==================================================

    public async Task<PontoRefresco?> BuscarPontoAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.PontosRefresco
            .AsNoTracking()
            .FirstOrDefaultAsync(
                p => p.Id == id,
                cancellationToken);
    }

    // ==================================================
    // CREATE - CADASTRAR TOTEM
    // ==================================================

    public async Task<Totem> AdicionarAsync(
        Totem totem,
        CancellationToken cancellationToken = default)
    {
        await _context.Totens.AddAsync(
            totem,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);

        return totem;
    }

    // ==================================================
    // READ - LISTAR TOTENS
    // ==================================================

    public async Task<List<Totem>> ListarAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Totens
            .AsNoTracking()
            .OrderBy(t => t.Codigo)
            .ToListAsync(cancellationToken);
    }

    // ==================================================
    // READ - BUSCAR POR ID
    // ==================================================

    public async Task<Totem?> BuscarPorIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Totens
            .FirstOrDefaultAsync(
                t => t.Id == id,
                cancellationToken);
    }

    // ==================================================
    // UPDATE - ATUALIZAR
    // ==================================================

    public async Task<Totem> AtualizarAsync(
        Totem totem,
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(
            cancellationToken);

        return totem;
    }

    // ==================================================
    // VERIFICAR HISTORICO DE LEITURAS
    // ==================================================

    public async Task<bool> PossuiLeiturasAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Leituras
            .AnyAsync(
                l => l.TotemId == id,
                cancellationToken);
    }

    // ==================================================
    // DELETE - EXCLUIR TOTEM
    // ==================================================

    public async Task ExcluirAsync(
        Totem totem,
        CancellationToken cancellationToken = default)
    {
        _context.Totens.Remove(totem);

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}
