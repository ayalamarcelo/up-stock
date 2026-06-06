using UpStock.Models;

namespace UpStock.Services;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(Guid id);
    Task<Category> CreateAsync(Category category);
    Task<bool> UpdateAsync(Guid id, Category category);
    Task<bool> DeleteAsync(Guid id);
}