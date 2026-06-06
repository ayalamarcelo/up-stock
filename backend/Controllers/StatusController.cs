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

    // GET: api/status
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Status>>> GetStatuses()
    {
        var statuses = await _statusService.GetAllAsync();
        return Ok(statuses);
    }

    // GET: api/status/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Status>> GetStatus(Guid id)
    {
        var status = await _statusService.GetByIdAsync(id);
        if (status == null) return NotFound(new { message = "Estado no encontrado" });

        return Ok(status);
    }

    // POST: api/status
    [HttpPost]
    public async Task<ActionResult<Status>> PostStatus(Status status)
    {
        var createdStatus = await _statusService.CreateAsync(status);
        return CreatedAtAction(nameof(GetStatus), new { id = createdStatus.statusid }, createdStatus); // Ajustado acá
    }

    // PUT: api/status/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutStatus(Guid id, Status status)
    {
        var result = await _statusService.UpdateAsync(id, status);
        if (!result) return BadRequest(new { message = "Error al actualizar el estado" });

        return NoContent();
    }

    // DELETE: api/status/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStatus(Guid id)
    {
        var result = await _statusService.DeleteAsync(id);
        if (!result) return NotFound(new { message = "Estado no encontrado para eliminar" });

        return NoContent();
    }
}