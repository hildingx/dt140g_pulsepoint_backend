using Microsoft.EntityFrameworkCore;
using PulsePoint.Data;
using PulsePoint.Models;

namespace PulsePoint.Services
{
    public class WorkplaceService : IWorkplaceService
    {
        private readonly PulsePointDbContext _context;

        public WorkplaceService(PulsePointDbContext context)
        {
            _context = context;
        }

        public async Task<List<Workplace>> GetAllAsync()
        {
            return await _context.Workplaces.ToListAsync();
        }

        public async Task<Workplace?> GetByIdAsync(int id)
        {
            return await _context.Workplaces.FindAsync(id);
        }

        public async Task<Workplace> CreateAsync(Workplace workplace)
        {
            _context.Workplaces.Add(workplace);
            await _context.SaveChangesAsync();
            return workplace;
        }

        public async Task<bool> UpdateAsync(Workplace workplace)
        {
            if (!_context.Workplaces.Any(w => w.Id == workplace.Id))
                return false;

            _context.Entry(workplace).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var workplace = await _context.Workplaces.FindAsync(id);
            if (workplace == null) return false;

            _context.Workplaces.Remove(workplace);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
