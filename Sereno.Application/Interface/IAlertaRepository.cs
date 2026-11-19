
using Sereno.Domain.Entities;

namespace Sereno.Application.Interfaces;

public interface IAlertaRepository
{
    Task<List<Alerta>> ListarAsync(
        bool? ativos = true,
        string? totemCodigo = null,
        CancellationToken cancellationToken = default);
}
