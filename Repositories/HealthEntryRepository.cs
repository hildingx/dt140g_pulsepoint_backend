using Microsoft.EntityFrameworkCore;
using PulsePoint.Data;
using PulsePoint.Models;

namespace PulsePoint.Repositories
{
    /// <summary>
    /// Repository för HealthEntry. Hanterar all direkt kommunikation med databasen.
    /// Används av HealthEntryService för att utföra CRUD-operationer på hälsoregistreringar.
    /// </summary>
    public class HealthEntryRepository(PulsePointDbContext context) : IHealthEntryRepository
    {
        private readonly PulsePointDbContext _context = context;

        /// <summary>
        /// Hämtar alla hälsoregistreringar för en viss användare.
        /// </summary>
        public async Task<List<HealthEntry>> GetByUserIdAsync(int userId)
        {
            return await _context.HealthEntries
                .Where(e => e.UserId == userId)
                .ToListAsync();
        }

        /// <summary>
        /// Hämtar en specifik hälsoregistrering utifrån dess ID och användarens ID.
        /// </summary>
        public async Task<HealthEntry?> GetByIdAsync(int id, int userId)
        {
            return await _context.HealthEntries
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
        }

        /// <summary>
        /// Lägger till en ny hälsoregistrering i databasen.
        /// </summary>
        public async Task AddAsync(HealthEntry entry)
        {
            _context.HealthEntries.Add(entry);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Uppdaterar en befintlig hälsoregistrering.
        /// </summary>
        public async Task<bool> UpdateAsync(HealthEntry entry)
        {
            _context.HealthEntries.Update(entry);

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                // TODO: logga felet internt
                return false;
            }
        }

        /// <summary>
        /// Tar bort en hälsoregistrering om den tillhör angiven användare.
        /// </summary>
        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var entry = await GetByIdAsync(id, userId);
            if (entry == null) return false;

            _context.HealthEntries.Remove(entry);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Kontrollerar om en registrering med angivet ID finns.
        /// </summary>
        public bool Exists(int id)
        {
            return _context.HealthEntries.Any(e => e.Id == id);
        }

        /// <summary>
        /// Hämtar alla hälsoregistreringar för ett visst workplace (via användarnas koppling).
        /// </summary>
        public async Task<List<HealthEntry>> GetByWorkplaceIdAsync(int workplaceId)
        {
            return await _context.HealthEntries
                .Where(e => e.User.WorkplaceId == workplaceId)
                .ToListAsync();
        }
    }
}
