using Microsoft.EntityFrameworkCore;
using PulsePoint.Data;
using PulsePoint.Models;

namespace PulsePoint.Services
{
    /// <summary>
    /// Repository för Workplace – hanterar all direkt kommunikation med databasen.
    /// Används av WorkplaceService för att utföra CRUD-operationer.
    /// </summary>
    public class WorkplaceRepository(PulsePointDbContext context) : IWorkplaceRepository
    {
        private readonly PulsePointDbContext _context = context;

        /// <summary>
        /// Hämtar alla arbetsplatser från databasen.
        /// </summary>
        /// <returns>En lista med alla arbetsplatser.</returns>
        public async Task<List<Workplace>> GetAllAsync()
        {
            return await _context.Workplaces.ToListAsync();
        }

        /// <summary>
        /// Hämtar en arbetsplats baserat på ID.
        /// </summary>
        /// <param name="id">ID för arbetsplatsen som ska hämtas.</param>
        /// <returns>Arbetsplatsen om den finns, annars null.</returns>
        public async Task<Workplace?> GetByIdAsync(int id)
        {
            return await _context.Workplaces.FindAsync(id);
        }

        /// <summary>
        /// Lägger till en ny arbetsplats i databasen.
        /// </summary>
        /// <param name="workplace">Arbetsplatsen som ska sparas.</param>
        /// <returns>Den sparade arbetsplatsen inklusive tilldelat ID.</returns>
        public async Task<Workplace> AddAsync(Workplace workplace)
        {
            _context.Workplaces.Add(workplace);
            await _context.SaveChangesAsync();
            return workplace;
        }

        /// <summary>
        /// Uppdaterar en befintlig arbetsplats.
        /// </summary>
        /// <param name="id">ID för arbetsplatsen som ska uppdateras.</param>
        /// <param name="updated">Det uppdaterade arbetsplatsobjektet.</param>
        /// <returns>True om uppdatering lyckades, annars false.</returns>
        public async Task<bool> UpdateAsync(int id, Workplace updated)
        {
            var existing = await _context.Workplaces.FindAsync(id);
            if (existing == null) return false;

            existing.Name = updated.Name;
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Tar bort en arbetsplats från databasen.
        /// </summary>
        /// <param name="id">ID för arbetsplatsen som ska tas bort.</param>
        /// <returns>True om borttagningen lyckades, annars false.</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Workplaces.FindAsync(id);
            if (existing == null) return false;

            _context.Workplaces.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
