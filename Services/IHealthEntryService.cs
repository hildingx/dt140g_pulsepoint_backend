using PulsePoint.Models.DTOs;

namespace PulsePoint.Services
{
    public interface IHealthEntryService
    {
        // GET
        Task<List<HealthEntryResponseDto>> GetEntriesForUserAsync(int userId);

        // GET :id
        Task<List<HealthEntryResponseDto>> GetEntryByIdAsync(int id, int userId);

        // PUT :id
        Task<bool> UpdateEntryAsync(int id, int userId, HealthEntryRequestDto dto);

        // POST
        Task<HealthEntryResponseDto> CreateEntryAsync(int userId, HealthEntryRequestDto dto);

        // DELETE :id
        Task<bool> DeleteEntryAsync(int id, int userId);

        // GET: api/HealthEntries/stats/daily
        Task<List<DailyWorkplaceStatsDto>> GetDailyStatsForWorkplaceAsync(int managerUserId);

        bool EntryExists(int id);
    }
}
