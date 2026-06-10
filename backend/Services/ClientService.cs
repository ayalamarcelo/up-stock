using Microsoft.EntityFrameworkCore;
using UpStock.Data;
using UpStock.Models;
using UpStock.Interfaces;

namespace UpStock.Services;

public class ClientService : IClientService
{
    private readonly AppDbContext _context;

    public ClientService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        return await _context.Clients.Where(c => c.IsActive).ToListAsync();
    }

    public async Task<Client?> GetByIdAsync(Guid id)
    {
        return await _context.Clients.FirstOrDefaultAsync(c => c.ClientID == id && c.IsActive);
    }

    public async Task<Client> CreateAsync(Client client)
    {
        client.ClientID = Guid.NewGuid();
        client.IsActive = true;
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        return client;
    }

    public async Task<bool> UpdateAsync(Guid id, Client client)
    {
        if (id != client.ClientID) return false;

        var existe = await _context.Clients.AnyAsync(c => c.ClientID == id && c.IsActive);
        if (!existe) return false;

        _context.Entry(client).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var client = await _context.Clients.FirstOrDefaultAsync(c => c.ClientID == id && c.IsActive);
        if (client == null) return false;

        client.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }
}
