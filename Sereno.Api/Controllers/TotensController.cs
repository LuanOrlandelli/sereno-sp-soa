
using Microsoft.AspNetCore.Mvc;
using Sereno.Application.DTOs;
using Sereno.Application.Services;

namespace Sereno.Api.Controllers;

[ApiController]
[Route("api/totens")]
public class TotensController : ControllerBase
{
    private readonly TotemService _service;

    public TotensController(TotemService service)
    {
        _service = service;
    }

    // ==================================================
    // POST /api/totens
    // ==================================================

    [HttpPost]
    public async Task<ActionResult<TotemRespostaDto>> Criar(
        [FromBody] CriarTotemDto dto,
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
            return BadRequest(new { erro = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { erro = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { erro = ex.Message });
        }
    }

    // ==================================================
    // GET /api/totens
    // ==================================================

    [HttpGet]
    public async Task<ActionResult<List<TotemRespostaDto>>> Listar(
        CancellationToken cancellationToken)
    {
        var resultado = await _service.ListarAsync(
            cancellationToken);

        return Ok(resultado);
    }

    // ==================================================
    // GET /api/totens/{id}
    // ==================================================

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TotemRespostaDto>> BuscarPorId(
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
                    erro = "Totem não encontrado."
                });
            }

            return Ok(resultado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }
    }

    // ==================================================
    // PUT /api/totens/{id}
    // ==================================================

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TotemRespostaDto>> Atualizar(
        [FromRoute] Guid id,
        [FromBody] AtualizarTotemDto dto,
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
            return BadRequest(new { erro = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { erro = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { erro = ex.Message });
        }
    }

    // ==================================================
    // DELETE /api/totens/{id}
    // ==================================================

    [HttpDelete("{id:guid}")]
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
            return BadRequest(new { erro = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { erro = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { erro = ex.Message });
        }
    }
}
