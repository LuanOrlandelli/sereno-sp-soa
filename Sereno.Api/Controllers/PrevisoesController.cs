
using Microsoft.AspNetCore.Mvc;
using Sereno.Application.DTOs;
using Sereno.Application.Services;

namespace Sereno.Api.Controllers;

[ApiController]
[Route("api/previsoes")]
public class PrevisoesController : ControllerBase
{
    private readonly PrevisaoService _service;

    public PrevisoesController(
        PrevisaoService service)
    {
        _service = service;
    }

    // ==================================================
    // POST /api/previsoes
    // ==================================================

    [HttpPost]
    [ProducesResponseType(
        typeof(PrevisaoRespostaDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status409Conflict)]
    public async Task<ActionResult<PrevisaoRespostaDto>> Criar(
        [FromBody] CriarPrevisaoDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            var resultado = await _service.CriarAsync(
                dto,
                cancellationToken);

            return StatusCode(
                StatusCodes.Status201Created,
                resultado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                erro = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                erro = ex.Message
            });
        }
    }

    // ==================================================
    // GET /api/previsoes/amanha
    // ==================================================

    [HttpGet("amanha")]
    [ProducesResponseType(
        typeof(PrevisaoRespostaDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PrevisaoRespostaDto>> BuscarAmanha(
        CancellationToken cancellationToken)
    {
        var resultado = await _service.BuscarAmanhaAsync(
            cancellationToken);

        if (resultado is null)
        {
            return NotFound(new
            {
                erro = "Previsão para amanhã indisponível."
            });
        }

        return Ok(resultado);
    }

    // ==================================================
    // GET /api/previsoes/data/2026-11-18
    // ==================================================

    [HttpGet("data/{data}")]
    [ProducesResponseType(
        typeof(PrevisaoRespostaDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PrevisaoRespostaDto>> BuscarPorData(
        [FromRoute] string data,
        CancellationToken cancellationToken)
    {
        if (!DateOnly.TryParseExact(
            data,
            "yyyy-MM-dd",
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None,
            out DateOnly dataReferencia))
        {
            return BadRequest(new
            {
                erro = "Utilize a data no formato yyyy-MM-dd."
            });
        }

        var resultado = await _service.BuscarPorDataAsync(
            dataReferencia,
            cancellationToken);

        if (resultado is null)
        {
            return NotFound(new
            {
                erro = "Previsão não encontrada para a data."
            });
        }

        return Ok(resultado);
    }
}
