using PulsePoint.Models;

namespace PulsePoint.Repositories
{
    /// <summary>
    /// Interface för HealthEntryRepository.
    /// Definierar metoder för att hantera hälsoregistreringar i databasen.
    /// Används av HealthEntryService för att utföra CRUD-operationer.
    /// </summary>
    public interface IHealthEntryRepository
    {
        /// <summary>
        /// Hämtar alla hälsoregistreringar för en specifik användare.
        /// </summary>
        Task<List<HealthEntry>> GetByUserIdAsync(int userId);

        /// <summary>
        /// Hämtar en specifik health entry baserat på id och användar-id.
        /// </summary>
        Task<HealthEntry?> GetByIdAsync(int id, int userId);

        /// <summary>
        /// Lägger till en ny health entry i databasen.
        /// </summary>
        Task AddAsync(HealthEntry entry);

        /// <summary>
        /// Uppdaterar en befintlig health entry.
        /// </summary>
        Task<bool> UpdateAsync(HealthEntry entry);

        /// <summary>
        /// Tar bort en health entry utifrån id och användar-id.
        /// </summary>
        Task<bool> DeleteAsync(int id, int userId);

        /// <summary>
        /// Kontrollerar om en health entry med angivet id finns.
        /// </summary>
        bool Exists(int id);

        /// <summary>
        /// Hämtar alla health entries för ett specifikt workplace.
        /// </summary>
        Task<List<HealthEntry>> GetByWorkplaceIdAsync(int workplaceId);
    }
}
