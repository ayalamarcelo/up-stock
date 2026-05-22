using UpStock.Models;

namespace UpStock.Services;

public interface IAssetService
{
    Task<IEnumerable<Asset>> GetAllAssetsAsync();
    Task<Asset?> AddAssetAsync(Asset asset);
}