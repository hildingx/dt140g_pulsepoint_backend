using PulsePoint.Models;

namespace PulsePoint.Services
{
    public interface IWorkplaceService
    {
        Task<List<Workplace>> GetAllAsync();
        Task<Workplace?> GetByIdAsync(int id);
        Task<Workplace> CreateAsync(Workplace workplace);
        Task<bool> UpdateAsync(Workplace workplace);
        Task<bool> DeleteAsync(int id);
    }
}
