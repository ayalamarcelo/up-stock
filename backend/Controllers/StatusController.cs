using UpStock.Models;
using UpStock.Services;
using Microsoft.AspNetCore.Mvc;

namespace UpStock.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatusController : ControllerBase
{
    private readonly IStatusService _statusService;

    public StatusController(IStatusService statusService)
    {
        _statusService = statusService;
    }

    // GET: api/Status
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Status>>> GetStatuses()
    {
        try
        {
            var statuses = await _statusService.GetAllAsync();

            if (!statuses.Any())
                return NotFound(new { message = "No hay estados registrados en el sistema." });

            return Ok(statuses);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
        }
    }

    // GET: api/Status/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Status>> GetStatus(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                return BadRequest(new { message = "El ID proporcionado no es válido." });

            var status = await _statusService.GetByIdAsync(id);

            if (status == null)
                return NotFound(new { message = $"No se encontró ningún estado con el ID {id}." });

            return Ok(status);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
        }
    }

    // POST: api/Status
    [HttpPost]
    public async Task<ActionResult<Status>> PostStatus(Status status)
    {
        try
        {
            if (status == null)
                return BadRequest(new { message = "El cuerpo de la solicitud no puede estar vacío." });

            if (string.IsNullOrWhiteSpace(status.namestatus))
                return BadRequest(new { message = "El nombre del estado es obligatorio." });

            var createdStatus = await _statusService.CreateAsync(status);
            return CreatedAtAction(nameof(GetStatus), new { id = createdStatus.statusid }, createdStatus);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
        }
    }

    // PUT: api/Status/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutStatus(Guid id, Status status)
    {
        try
        {
            if (id == Guid.Empty)
                return BadRequest(new { message = "El ID proporcionado no es válido." });

            if (id != status.statusid)
                return BadRequest(new { message = "El ID de la URL no coincide con el ID del cuerpo de la solicitud." });

            if (string.IsNullOrWhiteSpace(status.namestatus))
                return BadRequest(new { message = "El nombre del estado es obligatorio." });

            var existe = await _statusService.GetByIdAsync(id);
            if (existe == null)
                return NotFound(new { message = $"No se encontró ningún estado con el ID {id}." });

            var result = await _statusService.UpdateAsync(id, status);

            if (!result)
                return BadRequest(new { message = "Error al actualizar el estado." });

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
        }
    }

    // DELETE: api/Status/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStatus(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                return BadRequest(new { message = "El ID proporcionado no es válido." });

            var existe = await _statusService.GetByIdAsync(id);
            if (existe == null)
                return NotFound(new { message = $"No se encontró ningún estado con el ID {id}." });

            var result = await _statusService.DeleteAsync(id);

            if (!result)
                return NotFound(new { message = "Estado no encontrado para eliminar." });

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
        }
    }
}