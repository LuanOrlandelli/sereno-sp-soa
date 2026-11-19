
using Sereno.Domain.Entities;

namespace Sereno.Application.Interfaces;

public interface ILeituraRepository
{
    Task<Totem?> BuscarTotemPorCodigoAsync(
        string codigo,
        CancellationToken cancellationToken = default);

    Task<LeituraSensor> AdicionarAsync(
        LeituraSensor leitura,
        Alerta? alerta,
        CancellationToken cancellationToken = default);

    Task<LeituraSensor?> BuscarUltimaAsync(
        string codigoTotem,
        CancellationToken cancellationToken = default);
}
