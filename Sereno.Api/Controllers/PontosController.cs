
using Microsoft.AspNetCore.Mvc;
using Sereno.Application.DTOs;
using Sereno.Application.Services;

namespace Sereno.Api.Controllers;

[ApiController]
[Route("api/pontos")]
public class PontosController : ControllerBase
{
    private readonly PontoRefrescoService _service;

    public PontosController(
        PontoRefrescoService service)
    {
        _service = service;
    }

    // ==================================================
    // POST /api/pontos
    // ==================================================

    [HttpPost]
    [ProducesResponseType(
        typeof(PontoRefrescoRespostaDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PontoRefrescoRespostaDto>> Criar(
        [FromBody] CriarPontoRefrescoDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            var resultado = await _service.CriarAsync(
                dto,
                cancellationToken);

            return CreatedAtAction(
                nameof(BuscarPorId),
                new { id = resultado.Id },
                resultado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                erro = ex.Message
            });
        }
    }

    // ==================================================
    // GET /api/pontos
    // ==================================================

    [HttpGet]
    [ProducesResponseType(
        typeof(List<PontoRefrescoRespostaDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<List<PontoRefrescoRespostaDto>>> Listar(
        [FromQuery] bool? temAgua = null,
        [FromQuery] bool? temSombra = null,
        CancellationToken cancellationToken = default)
    {
        var resultado = await _service.ListarAsync(
            temAgua,
            temSombra,
            cancellationToken);

        return Ok(resultado);
    }

    // ==================================================
    // GET /api/pontos/proximos
    // ==================================================

    [HttpGet("proximos")]
    [ProducesResponseType(
        typeof(List<PontoRefrescoRespostaDto>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<PontoRefrescoRespostaDto>>> BuscarProximos(
        [FromQuery] double? lat,
        [FromQuery] double? lon,
        [FromQuery] int limite = 3,
        CancellationToken cancellationToken = default)
    {
        if (!lat.HasValue || !lon.HasValue)
        {
            return BadRequest(new
            {
                erro = "Informe latitude e longitude."
            });
        }

        try
        {
            var resultado = await _service.BuscarProximosAsync(
                lat.Value,
                lon.Value,
                limite,
                cancellationToken);

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

    // ==================================================
    // GET /api/pontos/{id}
    // ==================================================

    [HttpGet("{id:guid}")]
    [ProducesResponseType(
        typeof(PontoRefrescoRespostaDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PontoRefrescoRespostaDto>> BuscarPorId(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var resultado = await _service.BuscarPorIdAsync(
                id,
                cancellationToken);

            if (resultado is null)
            {
                return NotFound(new
                {
                    erro = "Ponto de refresco não encontrado."
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

    // ==================================================
    // PUT /api/pontos/{id}
    // ==================================================

    [HttpPut("{id:guid}")]
    [ProducesResponseType(
        typeof(PontoRefrescoRespostaDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status400BadRequest)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PontoRefrescoRespostaDto>> Atualizar(
        [FromRoute] Guid id,
        [FromBody] AtualizarPontoRefrescoDto dto,
        CancellationToken cancellationToken)
    {
        try
        {
            var resultado = await _service.AtualizarAsync(
                id,
                dto,
                cancellationToken);

            return Ok(resultado);
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
    }

    // ==================================================
    // DELETE /api/pontos/{id}
    // ==================================================

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(
        StatusCodes.Status204NoContent)]
    [ProducesResponseType(
        StatusCodes.Status404NotFound)]
    [ProducesResponseType(
        StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Excluir(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            await _service.ExcluirAsync(
                id,
                cancellationToken);

            return NoContent();
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
}
