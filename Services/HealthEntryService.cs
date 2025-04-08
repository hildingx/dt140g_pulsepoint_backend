using Microsoft.EntityFrameworkCore;
using PulsePoint.Data;
using PulsePoint.Models;
using PulsePoint.Models.DTOs;

namespace PulsePoint.Services
{
    public class HealthEntryService : IHealthEntryService
    {
        private readonly PulsePointDbContext _context;

        public HealthEntryService(PulsePointDbContext context)
        {
            _context = context;
        }

        public async Task<List<HealthEntryResponseDto>> GetEntriesForUserAsync(int userId)
        {
            return await _context.HealthEntries
                .Where(e => e.UserId == userId)
                .Select(e => new HealthEntryResponseDto
                {
                    Id = e.Id,
                    Mood = e.Mood,
                    Sleep = e.Sleep,
                    Stress = e.Stress,
                    Activity = e.Activity,
                    Nutrition = e.Nutrition,
                    Date = e.Date
                })
                .ToListAsync();
        }

        public async Task<HealthEntryResponseDto?> GetEntryByIdAsync(int id, int userId)
        {
            var entry = await _context.HealthEntries
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (entry == null) return null;

            return new HealthEntryResponseDto
            {
                Id = entry.Id,
                Mood = entry.Mood,
                Sleep = entry.Sleep,
                Stress = entry.Stress,
                Activity = entry.Activity,
                Nutrition = entry.Nutrition,
                Date = entry.Date
            };
        }

        public async Task<bool> UpdateEntryAsync(int id, int userId, HealthEntryRequestDto dto)
        {
            var entry = await _context.HealthEntries
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (entry == null)
                return false;

            // Uppdatera endast tillåtna fält
            entry.Mood = dto.Mood;
            entry.Sleep = dto.Sleep;
            entry.Stress = dto.Stress;
            entry.Activity = dto.Activity;
            entry.Nutrition = dto.Nutrition;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                // TODO: logga internt vid behov
                return false;
            }
        }

        public async Task<HealthEntryResponseDto> CreateEntryAsync(int userId, HealthEntryRequestDto dto)
        {
            var entry = new HealthEntry
            {
                Mood = dto.Mood,
                Sleep = dto.Sleep,
                Stress = dto.Stress,
                Activity = dto.Activity,
                Nutrition = dto.Nutrition,
                Date = DateTime.Today,
                UserId = userId
            };

            _context.HealthEntries.Add(entry);
            await _context.SaveChangesAsync();

            return new HealthEntryResponseDto
            {
                Id = entry.Id,
                Mood = entry.Mood,
                Sleep = entry.Sleep,
                Stress = entry.Stress,
                Activity = entry.Activity,
                Nutrition = entry.Nutrition,
                Date = entry.Date
            };
        }

        public async Task<bool> DeleteEntryAsync(int id, int userId)
        {
            var entry = await _context.HealthEntries
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (entry == null)
                return false;

            _context.HealthEntries.Remove(entry);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<DailyWorkplaceStatsDto>> GetDailyStatsForWorkplaceAsync(int managerUserId)
        {
            var manager = await _context.Users
                .Include(u => u.Workplace)
                .FirstOrDefaultAsync(u => u.Id == managerUserId);

            if (manager is null) return new();

            var workplaceId = manager.WorkplaceId;

            var dailyStats = await _context.HealthEntries
                .Where(e => e.User.WorkplaceId == workplaceId)
                .GroupBy(e => e.Date.Date)
                .Select(g => new DailyWorkplaceStatsDto
                {
                    Date = DateOnly.FromDateTime(g.Key),
                    AverageMood = g.Average(e => e.Mood),
                    AverageSleep = g.Average(e => e.Sleep),
                    AverageStress = g.Average(e => e.Stress),
                    AverageActivity = g.Average(e => e.Activity),
                    AverageNutrition = g.Average(e => e.Nutrition),
                    EntryCount = g.Count()
                })
                .OrderBy(s => s.Date)
                .ToListAsync();

            return dailyStats;
        }

        public bool EntryExists(int id)
        {
            return _context.HealthEntries.Any(e => e.Id == id);
        }
    }
}
