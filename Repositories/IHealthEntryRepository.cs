using PulsePoint.Models;

namespace PulsePoint.Repositories
{
    public interface IHealthEntryRepository
    {
        Task<List<HealthEntry>> GetByUserIdAsync(int userId);
        Task<HealthEntry?> GetByIdAsync(int id, int userId);
        Task AddAsync(HealthEntry entry);
        Task<bool> UpdateAsync(HealthEntry entry);
        Task<bool> DeleteAsync(int id, int userId);
        bool Exists(int id);
        Task<List<HealthEntry>> GetByWorkplaceIdAsync(int workplaceId);
    }
}
