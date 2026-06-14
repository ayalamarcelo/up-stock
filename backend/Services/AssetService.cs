using UpStock.Data;
using UpStock.Models;
using UpStock.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace UpStock.Services;

public class AssetService : IAssetService
{
    private readonly AppDbContext _context;

    public AssetService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Asset>> GetAllAsync()
    {
        return await _context.Assets.ToListAsync();
    }

    public async Task<Asset?> GetByIdAsync(Guid id)
    {
        return await _context.Assets.FindAsync(id);
    }

    public async Task<Asset> CreateAsync(Asset asset)
    {
        asset.AssetId = Guid.NewGuid();
        asset.IsDeleted = false;
        _context.Assets.Add(asset);
        await _context.SaveChangesAsync();
        return asset;
    }

    public async Task<bool> UpdateAsync(Guid id, Asset asset)
    {
        if (id != asset.AssetId) return false;

        _context.Entry(asset).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await AssetExists(id)) return false;
            throw;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var asset = await _context.Assets.FindAsync(id);
        if (asset == null) return false;

        asset.IsDeleted = true;
        _context.Entry(asset).State = EntityState.Modified;

        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<bool> AssetExists(Guid id)
    {
        return await _context.Assets.AnyAsync(e => e.AssetId == id);
    }
}