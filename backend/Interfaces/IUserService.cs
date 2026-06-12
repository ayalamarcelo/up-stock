using UpStock.Models;

namespace UpStock.Interfaces;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(Guid id);
    Task<User> CreateAsync(User user);
    Task<bool> UpdateAsync(Guid id, User user);
    Task<bool> DeleteAsync(Guid id);
}
