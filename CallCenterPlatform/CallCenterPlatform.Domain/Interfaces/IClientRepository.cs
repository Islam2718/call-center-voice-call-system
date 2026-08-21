using CallCenterPlatform.Domain.Entities;

namespace CallCenterPlatform.Domain.Interfaces;

public interface IClientRepository
{
    Task<Client?> GetByIdAsync(Guid id);
    Task<IEnumerable<Client>> GetAllAsync();
    Task<IEnumerable<Client>> GetActiveClientsAsync();
    Task<bool> CompanyNameExistsAsync(string companyName, Guid? excludeId = null);
    Task<bool> EmailExistsAsync(string email, Guid? excludeId = null);
    Task AddAsync(Client client);
    Task UpdateAsync(Client client);
    Task DeleteAsync(Guid id);  // Hard delete
    Task SoftDeleteAsync(Guid id);  // IsActive = false
}