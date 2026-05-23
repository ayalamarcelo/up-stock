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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Client>>> GetClients()
    {
        return await _context.Clients.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Client>> GetClient(Guid id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null) return NotFound();
        return client;
    }

    [HttpPost]
    public async Task<ActionResult<Client>> CreateClient(Client client)
    {
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetClient), new { id = client.ClientID }, client);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateClient(Guid id, Client client)
    {
        if (id != client.ClientID) return BadRequest();
        _context.Entry(client).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClient(Guid id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null) return NotFound();

        client.IsActive = false; // Soft Delete
        await _context.SaveChangesAsync();
        return NoContent();
    }
}