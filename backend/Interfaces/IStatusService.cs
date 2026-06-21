using UpStock.Models;

namespace UpStock.Interfaces;

public interface IStatusService
{
    Task<IEnumerable<Status>> GetAllAsync(int page, int pageSize);
    Task<Status?> GetByIdAsync(Guid id);
    Task<Status> CreateAsync(Status status);
    Task<bool> UpdateAsync(Guid id, Status status);
    Task<bool> DeleteAsync(Guid id);
}
