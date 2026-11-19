
using Sereno.Domain.Entities;

namespace Sereno.Application.Interfaces;

public interface IPontoRefrescoRepository
{
    // CREATE
    Task<PontoRefresco> AdicionarAsync(
        PontoRefresco ponto,
        CancellationToken cancellationToken = default);

    // READ - LISTAR
    Task<List<PontoRefresco>> ListarAsync(
        bool? temAgua = null,
        bool? temSombra = null,
        CancellationToken cancellationToken = default);

    // READ - BUSCAR POR ID
    Task<PontoRefresco?> BuscarPorIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    // UPDATE
    Task<PontoRefresco> AtualizarAsync(
        PontoRefresco ponto,
        CancellationToken cancellationToken = default);

    // VERIFICAR VINCULO COM TOTENS
    Task<bool> PossuiTotensAsync(
        Guid pontoId,
        CancellationToken cancellationToken = default);

    // DELETE
    Task ExcluirAsync(
        PontoRefresco ponto,
        CancellationToken cancellationToken = default);
}
