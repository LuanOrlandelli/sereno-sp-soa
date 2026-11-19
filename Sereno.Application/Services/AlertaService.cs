
using Sereno.Application.DTOs;
using Sereno.Application.Interfaces;
using Sereno.Domain.Entities;

namespace Sereno.Application.Services;

public class AlertaService
{
    private readonly IAlertaRepository _repository;

    public AlertaService(IAlertaRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<AlertaRespostaDto>> ListarAsync(
        bool? ativos = true,
        string? totemCodigo = null,
        CancellationToken cancellationToken = default)
    {
        var alertas = await _repository.ListarAsync(
            ativos,
            totemCodigo,
            cancellationToken);

        return alertas
            .Select(ConverterParaDto)
            .ToList();
    }

    private static AlertaRespostaDto ConverterParaDto(
        Alerta alerta)
    {
        return new AlertaRespostaDto
        {
            Id = alerta.Id,
            LeituraSensorId = alerta.LeituraSensorId,
            TotemCodigo = alerta.TotemCodigo,
            Origem = alerta.Origem,
            Nivel = alerta.Nivel.ToString(),
            Mensagem = alerta.Mensagem,
            Ativo = alerta.Ativo,
            CriadoEm = alerta.CriadoEm
        };
    }
}
