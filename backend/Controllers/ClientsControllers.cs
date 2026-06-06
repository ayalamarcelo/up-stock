using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpStock.Data;
using UpStock.Models;

namespace UpStock.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ClientsController(AppDbContext context) => _context = context;

    // GET: api/Clients
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Client>>> GetClients()
    {
        try
        {
            var clients = await _context.Clients.Where(c => c.IsActive).ToListAsync();

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

            var client = await _context.Clients.FirstOrDefaultAsync(c => c.ClientID == id && c.IsActive);

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

            var existe = await _context.Clients.AnyAsync(c => c.DniCuit == client.DniCuit && c.IsActive);
            if (existe)
                return Conflict($"Ya existe un cliente con el DNI/CUIT '{client.DniCuit}'.");

            client.ClientID = Guid.NewGuid();
            client.IsActive = true;

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClient), new { id = client.ClientID }, client);
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

            var existe = await _context.Clients.AnyAsync(c => c.ClientID == id && c.IsActive);
            if (!existe)
                return NotFound($"No se encontró ningún cliente con el ID {id}.");

            var dniDuplicado = await _context.Clients.AnyAsync(c => c.DniCuit == client.DniCuit && c.ClientID != id && c.IsActive);
            if (dniDuplicado)
                return Conflict($"Ya existe otro cliente con el DNI/CUIT '{client.DniCuit}'.");

            _context.Entry(client).State = EntityState.Modified;
            await _context.SaveChangesAsync();

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

            var client = await _context.Clients.FirstOrDefaultAsync(c => c.ClientID == id && c.IsActive);

            if (client == null)
                return NotFound($"No se encontró ningún cliente con el ID {id}.");

            client.IsActive = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }
}