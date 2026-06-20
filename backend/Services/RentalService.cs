using Microsoft.EntityFrameworkCore;
using UpStock.Data;
using UpStock.Models;
using UpStock.Interfaces;

namespace UpStock.Services;

public class RentalService : IRentalService
{
    private readonly AppDbContext _context;

    public RentalService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Rental>> GetAllAsync(int page, int pageSize)
    {
        // Incluimos las relaciones para que el JSON devuelva los datos completos
        return await _context.Rentals
            .Include(r => r.Client)
            .Include(r => r.Status)
            .Include(r => r.User)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Rental?> GetByIdAsync(Guid id)
    {
        return await _context.Rentals
            .Include(r => r.Client)
            .Include(r => r.Status)
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.RentalID == id);
    }

    public async Task<Rental> CreateAsync(Rental rental)
    {
        rental.RentalID = Guid.NewGuid();
        _context.Rentals.Add(rental);
        await _context.SaveChangesAsync();
        return rental;
    }

    public async Task<bool> UpdateAsync(Guid id, Rental rental)
    {
        var existingRental = await _context.Rentals.FindAsync(id);
        if (existingRental == null) return false;

        existingRental.StatusID = rental.StatusID;
        existingRental.RentalDateExpected = rental.RentalDateExpected;
        
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var rental = await _context.Rentals.FindAsync(id);
        if (rental == null) return false;

        _context.Rentals.Remove(rental);
        await _context.SaveChangesAsync();
        return true;
    }
}