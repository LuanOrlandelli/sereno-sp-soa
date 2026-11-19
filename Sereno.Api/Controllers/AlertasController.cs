
using Microsoft.AspNetCore.Mvc;
using Sereno.Application.DTOs;
using Sereno.Application.Services;

namespace Sereno.Api.Controllers;

[ApiController]
[Route("api/alertas")]
public class AlertasController : ControllerBase
{
    private readonly AlertaService _service;

    public AlertasController(AlertaService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(List<AlertaRespostaDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<List<AlertaRespostaDto>>> Listar(
        [FromQuery] bool? ativos = true,
        [FromQuery] string? totemCodigo = null,
        CancellationToken cancellationToken = default)
    {
        var resultado = await _service.ListarAsync(
            ativos,
            totemCodigo,
            cancellationToken);

        return Ok(resultado);
    }
}
