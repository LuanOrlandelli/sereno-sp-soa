
using Sereno.Application.DTOs;
using Sereno.Application.Interfaces;
using Sereno.Domain.Entities;

namespace Sereno.Application.Services;

public class TotemService
{
    private readonly ITotemRepository _repository;

    public TotemService(ITotemRepository repository)
    {
        _repository = repository;
    }

    // ==================================================
    // CREATE - CADASTRAR TOTEM
    // ==================================================

    public async Task<TotemRespostaDto> CriarAsync(
        CriarTotemDto dto,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(dto.Codigo))
        {
            throw new ArgumentException(
                "O código do totem é obrigatório.");
        }

        var codigo = dto.Codigo.Trim();

        if (codigo.Length < 3 || codigo.Length > 50)
        {
            throw new ArgumentException(
                "O código deve ter entre 3 e 50 caracteres.");
        }

        if (await _repository.ExisteCodigoAsync(
            codigo, cancellationToken))
        {
            throw new InvalidOperationException(
                "Já existe um totem com esse código.");
        }

        await ValidarPontoAsync(
            dto.PontoRefrescoId,
            cancellationToken);

        var totem = new Totem(
            codigo,
            dto.PontoRefrescoId);

        var salvo = await _repository.AdicionarAsync(
            totem,
            cancellationToken);

        return ConverterParaDto(salvo);
    }

    // ==================================================
    // READ - LISTAR TODOS
    // ==================================================

    public async Task<List<TotemRespostaDto>> ListarAsync(
        CancellationToken cancellationToken = default)
    {
        var totens = await _repository.ListarAsync(
            cancellationToken);

        return totens
            .Select(ConverterParaDto)
            .ToList();
    }

    // ==================================================
    // READ - BUSCAR POR ID
    // ==================================================

    public async Task<TotemRespostaDto?> BuscarPorIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Identificador do totem inválido.");
        }

        var totem = await _repository.BuscarPorIdAsync(
            id,
            cancellationToken);

        return totem is null
            ? null
            : ConverterParaDto(totem);
    }

    // ==================================================
    // UPDATE - ATUALIZAR
    // ==================================================

    public async Task<TotemRespostaDto> AtualizarAsync(
        Guid id,
        AtualizarTotemDto dto,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Identificador do totem inválido.");
        }

        if (string.IsNullOrWhiteSpace(dto.Codigo))
        {
            throw new ArgumentException(
                "O código do totem é obrigatório.");
        }

        var codigo = dto.Codigo.Trim();

        if (codigo.Length < 3 || codigo.Length > 50)
        {
            throw new ArgumentException(
                "O código deve ter entre 3 e 50 caracteres.");
        }

        var totem = await _repository.BuscarPorIdAsync(
            id,
            cancellationToken);

        if (totem is null)
        {
            throw new KeyNotFoundException(
                "Totem não encontrado.");
        }

        if (await _repository.ExisteOutroCodigoAsync(
            codigo,
            id,
            cancellationToken))
        {
            throw new InvalidOperationException(
                "Já existe outro totem com esse código.");
        }

        await ValidarPontoAsync(
            dto.PontoRefrescoId,
            cancellationToken);

        totem.Atualizar(
            codigo,
            dto.PontoRefrescoId,
            dto.Ativo);

        var atualizado = await _repository.AtualizarAsync(
            totem,
            cancellationToken);

        return ConverterParaDto(atualizado);
    }

    // ==================================================
    // DELETE - EXCLUIR
    // ==================================================

    public async Task ExcluirAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                "Identificador do totem inválido.");
        }

        var totem = await _repository.BuscarPorIdAsync(
            id,
            cancellationToken);

        if (totem is null)
        {
            throw new KeyNotFoundException(
                "Totem não encontrado.");
        }

        bool possuiLeituras =
            await _repository.PossuiLeiturasAsync(
                id,
                cancellationToken);

        if (possuiLeituras)
        {
            throw new InvalidOperationException(
                "Este totem possui histórico de leituras. " +
                "Desative o dispositivo em vez de excluí-lo.");
        }

        await _repository.ExcluirAsync(
            totem,
            cancellationToken);
    }

    // ==================================================
    // VALIDAR PONTO DE REFRESCO
    // ==================================================

    private async Task ValidarPontoAsync(
        Guid? pontoRefrescoId,
        CancellationToken cancellationToken)
    {
        if (!pontoRefrescoId.HasValue)
            return;

        var ponto = await _repository.BuscarPontoAsync(
            pontoRefrescoId.Value,
            cancellationToken);

        if (ponto is null)
        {
            throw new KeyNotFoundException(
                "Ponto de refresco não encontrado.");
        }
    }

    // ==================================================
    // CONVERTER ENTIDADE PARA DTO
    // ==================================================

    private static TotemRespostaDto ConverterParaDto(
        Totem totem)
    {
        return new TotemRespostaDto
        {
            Id = totem.Id,
            Codigo = totem.Codigo,
            Ativo = totem.Ativo,
            PontoRefrescoId = totem.PontoRefrescoId
        };
    }
}
