using UpStock.Models;

namespace UpStock.Interfaces;

public interface IClientService
{
    Task<IEnumerable<Client>> GetAllAsync();
    Task<Client?> GetByIdAsync(Guid id);
    Task<Client> CreateAsync(Client client);
    Task<bool> UpdateAsync(Guid id, Client client);
    Task<bool> DeleteAsync(Guid id);
}
