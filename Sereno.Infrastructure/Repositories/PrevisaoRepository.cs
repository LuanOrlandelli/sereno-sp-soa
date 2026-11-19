
using Microsoft.EntityFrameworkCore;
using Sereno.Application.Interfaces;
using Sereno.Domain.Entities;
using Sereno.Infrastructure.Data;

namespace Sereno.Infrastructure.Repositories;

public class PrevisaoRepository : IPrevisaoRepository
{
    private readonly SerenoDbContext _context;

    public PrevisaoRepository(
        SerenoDbContext context)
    {
        _context = context;
    }

    // ==================================================
    // VERIFICAR SE EXISTE PREVISAO NA DATA
    // ==================================================

    public async Task<bool> ExisteParaDataAsync(
        DateOnly dataReferencia,
        CancellationToken cancellationToken = default)
    {
        return await _context.PrevisoesCalor
            .AnyAsync(
                p => p.DataReferencia == dataReferencia,
                cancellationToken);
    }

    // ==================================================
    // ADICIONAR UMA NOVA PREVISAO
    // ==================================================

    public async Task<PrevisaoCalor> AdicionarAsync(
        PrevisaoCalor previsao,
        CancellationToken cancellationToken = default)
    {
        await _context.PrevisoesCalor.AddAsync(
            previsao,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);

        return previsao;
    }

    // ==================================================
    // BUSCAR PREVISAO POR DATA
    // ==================================================

    public async Task<PrevisaoCalor?> BuscarPorDataAsync(
        DateOnly dataReferencia,
        CancellationToken cancellationToken = default)
    {
        return await _context.PrevisoesCalor
            .AsNoTracking()
            .FirstOrDefaultAsync(
                p => p.DataReferencia == dataReferencia,
                cancellationToken);
    }
}
