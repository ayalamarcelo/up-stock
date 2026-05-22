using Microsoft.EntityFrameworkCore;
using UpStock.Data;
using UpStock.Models;

namespace UpStock.Services;

public class AssetService : IAssetService
{
    private readonly AppDbContext _context;

    public AssetService(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Asset>> GetAllAssetsAsync()
    {
        return await _context.Assets.ToListAsync();
    }

    public async Task<Asset?> AddAssetAsync(Asset asset)
    {
        // Aquí podrías agregar reglas de negocio, ej:
        // if (string.IsNullOrEmpty(asset.Nombre)) return null;
        
        _context.Assets.Add(asset);
        await _context.SaveChangesAsync();
        return asset;
    }
}