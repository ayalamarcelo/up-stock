using UpStock.Models;

namespace UpStock.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllAsync(int page, int pageSize);
    Task<Category?> GetByIdAsync(Guid id);
    Task<Category> CreateAsync(Category category);
    Task<bool> UpdateAsync(Guid id, Category category);
    Task<bool> DeleteAsync(Guid id);
}
