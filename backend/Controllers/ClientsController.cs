using Microsoft.AspNetCore.Mvc;
using UpStock.Models;
using UpStock.Interfaces;

namespace UpStock.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService) => _clientService = clientService;

    // GET: api/Clients
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Client>>> GetClients()
    {
        try
        {
            var clients = await _clientService.GetAllAsync();

            if (!clients.Any())
                return NotFound("No hay clientes registrados en el sistema.");

            return Ok(clients);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    // GET: api/Clients/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Client>> GetClient(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                return BadRequest("El ID proporcionado no es válido.");

            var client = await _clientService.GetByIdAsync(id);

            if (client == null)
                return NotFound($"No se encontró ningún cliente con el ID {id}.");

            return Ok(client);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    // POST: api/Clients
    [HttpPost]
    public async Task<ActionResult<Client>> CreateClient(Client client)
    {
        try
        {
            if (client == null)
                return BadRequest("El cuerpo de la solicitud no puede estar vacío.");

            if (string.IsNullOrWhiteSpace(client.Name))
                return BadRequest("El nombre del cliente es obligatorio.");

            if (string.IsNullOrWhiteSpace(client.DniCuit))
                return BadRequest("El DNI/CUIT del cliente es obligatorio.");

            if (string.IsNullOrWhiteSpace(client.Phone))
                return BadRequest("El teléfono del cliente es obligatorio.");

            var created = await _clientService.CreateAsync(client);
            return CreatedAtAction(nameof(GetClient), new { id = created.ClientID }, created);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    // PUT: api/Clients/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClient(Guid id, Client client)
    {
        try
        {
            if (id == Guid.Empty)
                return BadRequest("El ID proporcionado no es válido.");

            if (id != client.ClientID)
                return BadRequest("El ID de la URL no coincide con el ID del cuerpo de la solicitud.");

            if (string.IsNullOrWhiteSpace(client.Name))
                return BadRequest("El nombre del cliente es obligatorio.");

            if (string.IsNullOrWhiteSpace(client.DniCuit))
                return BadRequest("El DNI/CUIT del cliente es obligatorio.");

            var updated = await _clientService.UpdateAsync(id, client);
            if (!updated)
                return NotFound($"No se encontró ningún cliente con el ID {id}.");

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }

    // DELETE: api/Clients/5 (Soft Delete)
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClient(Guid id)
    {
        try
        {
            if (id == Guid.Empty)
                return BadRequest("El ID proporcionado no es válido.");

            var deleted = await _clientService.DeleteAsync(id);
            if (!deleted)
                return NotFound($"No se encontró ningún cliente con el ID {id}.");

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }
}