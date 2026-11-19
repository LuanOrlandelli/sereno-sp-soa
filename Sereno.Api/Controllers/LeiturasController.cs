
using Microsoft.AspNetCore.Mvc;
using Sereno.Application.DTOs;
using Sereno.Application.Services;

namespace Sereno.Api.Controllers;

[ApiController]
[Route("api/leituras")]
public class LeiturasController : ControllerBase
{
    private readonly LeituraService _service;

    public LeiturasController(LeituraService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType(
        typeof(LeituraRespostaDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    [ProducesResponseType(
        StatusCodes.Status409Conflict)]
    public async Task<ActionResult<LeituraRespostaDto>> Criar(
        [FromBody] CriarLeituraDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            var resultado = await _service.CriarAsync(
                dto, cancellationToken);

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
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
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

    [HttpGet("ultima")]
    [ProducesResponseType(
        typeof(LeituraRespostaDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<LeituraRespostaDto>> BuscarUltima(
        [FromQuery] string totemCodigo,
        CancellationToken cancellationToken)
    {
        try
        {
            var resultado = await _service.BuscarUltimaAsync(
                totemCodigo, cancellationToken);

            if (resultado is null)
            {
                return NotFound(new
                {
                    erro = "Nenhuma leitura encontrada."
                });
            }

            return Ok(resultado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                erro = ex.Message
            });
        }
    }
}
