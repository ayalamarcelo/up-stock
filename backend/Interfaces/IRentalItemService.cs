using UpStock.Models;

namespace UpStock.Interfaces;

public interface IRentalItemService
{
    Task<IEnumerable<RentalItem>> GetByRentalIdAsync(Guid rentalId);
    Task<bool> AddItemAsync(RentalItem item);
    Task<bool> RemoveItemAsync(Guid itemId);
}
