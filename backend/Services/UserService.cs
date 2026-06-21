using Microsoft.EntityFrameworkCore;
using UpStock.Data;
using UpStock.Models;
using UpStock.Interfaces;

namespace UpStock.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllAsync(int page, int pageSize)
    {
        return await _context.Users
        .Where(u => u.IsActive)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserID == id && u.IsActive);
    }

    public async Task<User> CreateAsync(User user)
    {
        user.UserID = Guid.NewGuid();
        user.IsActive = true;
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> UpdateAsync(Guid id, User user)
    {
        if (id != user.UserID) return false;

        var existe = await _context.Users.AnyAsync(u => u.UserID == id && u.IsActive);
        if (!existe) return false;

        _context.Entry(user).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == id && u.IsActive);
        if (user == null) return false;

        user.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }
}
