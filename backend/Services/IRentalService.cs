using UpStock.Models;

namespace UpStock.Services;

public interface IRentalService
{
    Task<IEnumerable<Rental>> GetAllAsync();
    Task<Rental?> GetByIdAsync(Guid id);
    Task<Rental> CreateAsync(Rental rental);
    Task<bool> UpdateAsync(Guid id, Rental rental);
    Task<bool> DeleteAsync(Guid id);
}