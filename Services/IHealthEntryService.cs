using PulsePoint.Models;
using PulsePoint.Models.DTOs;

namespace PulsePoint.Services
{
    /// <summary>
    /// Interface för tjänst som hanterar användares hälsoinmatningar och statistik.
    /// Implementeras av HealthEntryService.
    /// </summary>
    public interface IHealthEntryService
    {
        /// <summary>
        /// Hämtar alla hälsoinmatningar för en specifik användare.
        /// </summary>
        /// <param name="userId">Användarens ID</param>
        /// <returns>Lista med hälsoinmatningar</returns>
        Task<List<HealthEntryResponseDto>> GetEntriesForUserAsync(int userId);

        /// <summary>
        /// Hämtar en specifik hälsoinmatning om den tillhör rätt användare.
        /// </summary>
        /// <param name="id">ID för inmatningen</param>
        /// <param name="userId">Inloggade användarens ID</param>
        /// <returns>Hälsoinmatning eller null</returns>
        Task<HealthEntryResponseDto?> GetEntryByIdAsync(int id, int userId);

        /// <summary>
        /// Uppdaterar en specifik hälsoinmatning om den tillhör rätt användare.
        /// </summary>
        /// <param name="id">ID för inmatningen</param>
        /// <param name="userId">Inloggade användarens ID</param>
        /// <param name="dto">Uppdaterade värden</param>
        /// <returns>True om uppdatering lyckades, annars false</returns>
        Task<bool> UpdateEntryAsync(int id, int userId, HealthEntryRequestDto dto);

        /// <summary>
        /// Skapar en ny hälsoinmatning för den aktuella användaren.
        /// </summary>
        /// <param name="userId">Inloggade användarens ID</param>
        /// <param name="dto">Inmatade värden</param>
        /// <returns>DTO med sparade värden och ID</returns>
        Task<HealthEntryResponseDto> CreateEntryAsync(int userId, HealthEntryRequestDto dto);

        /// <summary>
        /// Tar bort en hälsoinmatning om den tillhör rätt användare.
        /// </summary>
        /// <param name="id">ID för inmatningen</param>
        /// <param name="userId">Inloggade användarens ID</param>
        /// <returns>True om borttagning lyckades, annars false</returns>
        Task<bool> DeleteEntryAsync(int id, int userId);

        /// <summary>
        /// Hämtar aggregerad statistik per dag för samtliga användare
        /// kopplade till en managers arbetsplats.
        /// </summary>
        /// <param name="managerUserId">Managerns användar-ID</param>
        /// <returns>Lista med daglig statistik</returns>
        Task<List<DailyWorkplaceStatsDto>> GetDailyStatsForWorkplaceAsync(int managerUserId);

        /// <summary>
        /// Kollar om en hälsoinmatning med angivet ID existerar.
        /// </summary>
        /// <param name="id">ID för inmatningen</param>
        /// <returns>True om inmatningen finns</returns>
        bool EntryExists(int id);
    }
}
