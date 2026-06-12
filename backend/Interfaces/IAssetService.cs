using UpStock.Models;

namespace UpStock.Interfaces;

public interface IAssetService
{
    Task<IEnumerable<Asset>> GetAllAsync();
    Task<Asset?> GetByIdAsync(Guid id);
    Task<Asset> CreateAsync(Asset asset);
    Task<bool> UpdateAsync(Guid id, Asset asset);
    Task<bool> DeleteAsync(Guid id);
}
