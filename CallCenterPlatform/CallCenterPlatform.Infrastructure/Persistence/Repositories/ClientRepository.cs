using Microsoft.EntityFrameworkCore;
using CallCenterPlatform.Domain.Entities;
using CallCenterPlatform.Domain.Interfaces;

namespace CallCenterPlatform.Infrastructure.Persistence.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly AppDbContext _context;

    public ClientRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Client?> GetByIdAsync(Guid id)
        => await _context.Clients.FindAsync(id);

    public async Task<IEnumerable<Client>> GetAllAsync()
        => await _context.Clients.ToListAsync();

    public async Task<IEnumerable<Client>> GetActiveClientsAsync()
        => await _context.Clients.Where(c => c.IsActive).ToListAsync();

    public async Task<bool> CompanyNameExistsAsync(string companyName, Guid? excludeId = null)
    {
        var query = _context.Clients.Where(c => c.CompanyName == companyName);
        if (excludeId.HasValue)
            query = query.Where(c => c.Id != excludeId.Value);
        return await query.AnyAsync();
    }

    public async Task<bool> EmailExistsAsync(string email, Guid? excludeId = null)
    {
        var query = _context.Clients.Where(c => c.Email == email);
        if (excludeId.HasValue)
            query = query.Where(c => c.Id != excludeId.Value);
        return await query.AnyAsync();
    }

    public async Task AddAsync(Client client)
    {
        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Client client)
    {
        _context.Clients.Update(client);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client != null)
        {
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
        }
    }

    public async Task SoftDeleteAsync(Guid id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client != null)
        {
            client.IsActive = false;
            client.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}