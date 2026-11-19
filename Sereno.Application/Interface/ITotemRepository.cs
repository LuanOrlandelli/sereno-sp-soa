
using Sereno.Domain.Entities;

namespace Sereno.Application.Interfaces;

public interface ITotemRepository
{
    // Verificar codigo existente
    Task<bool> ExisteCodigoAsync(
        string codigo,
        CancellationToken cancellationToken = default);

    // Verificar codigo em outro totem
    Task<bool> ExisteOutroCodigoAsync(
        string codigo,
        Guid idAtual,
        CancellationToken cancellationToken = default);

    // Buscar ponto de refresco
    Task<PontoRefresco?> BuscarPontoAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    // Cadastrar
    Task<Totem> AdicionarAsync(
        Totem totem,
        CancellationToken cancellationToken = default);

    // Listar
    Task<List<Totem>> ListarAsync(
        CancellationToken cancellationToken = default);

    // Buscar por ID
    Task<Totem?> BuscarPorIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    // Atualizar
    Task<Totem> AtualizarAsync(
        Totem totem,
        CancellationToken cancellationToken = default);

    // Verificar historico de leituras
    Task<bool> PossuiLeiturasAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    // Excluir
    Task ExcluirAsync(
        Totem totem,
        CancellationToken cancellationToken = default);
}
