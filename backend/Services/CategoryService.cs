using UpStock.Data;
using UpStock.Models;
using UpStock.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace UpStock.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        return await _context.Categories.FindAsync(id);
    }

    public async Task<Category> CreateAsync(Category category)
    {
        category.CategoryID = Guid.NewGuid();
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task<bool> UpdateAsync(Guid id, Category category)
    {
        if (id != category.CategoryID) return false;

        _context.Entry(category).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await CategoryExists(id)) return false;
            throw;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null) return false;

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<bool> CategoryExists(Guid id)
    {
        return await _context.Categories.AnyAsync(e => e.CategoryID == id);
    }
}