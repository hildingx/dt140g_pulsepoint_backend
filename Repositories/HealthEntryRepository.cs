using Microsoft.EntityFrameworkCore;
using PulsePoint.Data;
using PulsePoint.Models;

namespace PulsePoint.Repositories
{
    public class HealthEntryRepository : IHealthEntryRepository
    {
        private readonly PulsePointDbContext _context;

        public HealthEntryRepository(PulsePointDbContext context)
        {
            _context = context;
        }

        public async Task<List<HealthEntry>> GetByUserIdAsync(int userId)
        {
            return await _context.HealthEntries
                .Where(e => e.UserId == userId)
                .ToListAsync();
        }

        public async Task<HealthEntry?> GetByIdAsync(int id, int userId)
        {
            return await _context.HealthEntries
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
        }

        public async Task AddAsync(HealthEntry entry)
        {
            _context.HealthEntries.Add(entry);
            await _context.SaveChangesAsync();
        }

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
                // TODO: logga felet
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var entry = await GetByIdAsync(id, userId);
            if (entry == null) return false;

            _context.HealthEntries.Remove(entry);
            await _context.SaveChangesAsync();
            return true;
        }

        public bool Exists(int id)
        {
            return _context.HealthEntries.Any(e => e.Id == id);
        }

        public async Task<List<HealthEntry>> GetByWorkplaceIdAsync(int workplaceId)
        {
            return await _context.HealthEntries
                .Where(e => e.User.WorkplaceId == workplaceId)
                .ToListAsync();
        }
    }
}
