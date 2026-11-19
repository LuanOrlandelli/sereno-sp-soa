
using Sereno.Domain.Entities;

namespace Sereno.Application.Interfaces;

public interface IPrevisaoRepository
{
    Task<bool> ExisteParaDataAsync(
        DateOnly dataReferencia,
        CancellationToken cancellationToken = default);

    Task<PrevisaoCalor> AdicionarAsync(
        PrevisaoCalor previsao,
        CancellationToken cancellationToken = default);

    Task<PrevisaoCalor?> BuscarPorDataAsync(
        DateOnly dataReferencia,
        CancellationToken cancellationToken = default);
}
