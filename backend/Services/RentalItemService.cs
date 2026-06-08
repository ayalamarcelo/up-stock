using Microsoft.EntityFrameworkCore;
using UpStock.Data;
using UpStock.Models;

namespace UpStock.Services;

public class RentalItemService : IRentalItemService
{
    private readonly AppDbContext _context;

    public RentalItemService(AppDbContext context) => _context = context;

    public async Task<IEnumerable<RentalItem>> GetByRentalIdAsync(Guid rentalId)
    {
        return await _context.Set<RentalItem>()
            .Include(ri => ri.Asset)
            .Where(ri => ri.RentalID == rentalId)
            .ToListAsync();
    }

    public async Task<bool> AddItemAsync(RentalItem item)
    {
        item.RentalItemID = Guid.NewGuid();
        _context.Set<RentalItem>().Add(item);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> RemoveItemAsync(Guid itemId)
    {
        var item = await _context.Set<RentalItem>().FindAsync(itemId);
        if (item == null) return false;
        
        _context.Set<RentalItem>().Remove(item);
        return await _context.SaveChangesAsync() > 0;
    }
}