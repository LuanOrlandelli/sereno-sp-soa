
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sereno.Infrastructure.Data;

namespace Sereno.Api.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    private readonly SerenoDbContext _context;
    private readonly ILogger<HealthController> _logger;

    public HealthController(
        SerenoDbContext context,
        ILogger<HealthController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // ==================================================
    // GET /health
    // ==================================================

    [HttpGet]
    [ProducesResponseType(
        StatusCodes.Status200OK)]
    [ProducesResponseType(
        StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> Verificar(
        CancellationToken cancellationToken)
    {
        try
        {
            bool bancoDisponivel =
                await _context.Database.CanConnectAsync(
                    cancellationToken);

            if (!bancoDisponivel)
            {
                _logger.LogWarning(
                    "PostgreSQL indisponivel no health check.");

                return StatusCode(
                    StatusCodes.Status503ServiceUnavailable,
                    new
                    {
                        status = "Unhealthy",
                        api = "Online",
                        banco = "Indisponivel"
                    });
            }

            return Ok(new
            {
                status = "Healthy",
                api = "Online",
                banco = "Online"
            });
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Falha ao verificar a conexao com o banco.");

            return StatusCode(
                StatusCodes.Status503ServiceUnavailable,
                new
                {
                    status = "Unhealthy",
                    api = "Online",
                    banco = "Indisponivel"
                });
        }
    }
}
