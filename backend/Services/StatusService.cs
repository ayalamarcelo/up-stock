using UpStock.Data;
using UpStock.Models;
using UpStock.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace UpStock.Services;

public class StatusService : IStatusService
{
    private readonly AppDbContext _context;

    public StatusService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Status>> GetAllAsync()
    {
        return await _context.Statuses.ToListAsync();
    }

    public async Task<Status?> GetByIdAsync(Guid id)
    {
        return await _context.Statuses.FindAsync(id);
    }

    public async Task<Status> CreateAsync(Status status)
    {
        status.StatusId = Guid.NewGuid();
        _context.Statuses.Add(status);
        await _context.SaveChangesAsync();
        return status;
    }

    public async Task<bool> UpdateAsync(Guid id, Status status)
    {
        if (id != status.StatusId) return false;

        _context.Entry(status).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await StatusExists(id)) return false;
            throw;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var status = await _context.Statuses.FindAsync(id);
        if (status == null) return false;

        _context.Statuses.Remove(status);
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<bool> StatusExists(Guid id)
    {
        return await _context.Statuses.AnyAsync(e => e.StatusId == id);
    }
}