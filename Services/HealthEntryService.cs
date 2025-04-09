using Microsoft.EntityFrameworkCore;
using PulsePoint.Data;
using PulsePoint.Models;
using PulsePoint.Models.DTOs;
using PulsePoint.Repositories;

namespace PulsePoint.Services
{
    public class HealthEntryService : IHealthEntryService
    {
        private readonly IHealthEntryRepository _repo;
        private readonly PulsePointDbContext _context;

        public HealthEntryService(IHealthEntryRepository repo, PulsePointDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        public async Task<List<HealthEntryResponseDto>> GetEntriesForUserAsync(int userId)
        {
            var entries = await _repo.GetByUserIdAsync(userId);

            return entries.Select(e => new HealthEntryResponseDto
            {
                Id = e.Id,
                Mood = e.Mood,
                Sleep = e.Sleep,
                Stress = e.Stress,
                Activity = e.Activity,
                Nutrition = e.Nutrition,
                Date = e.Date
            }).ToList();
        }

        public async Task<HealthEntryResponseDto?> GetEntryByIdAsync(int id, int userId)
        {
            var entry = await _repo.GetByIdAsync(id, userId);

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
            var entry = await _repo.GetByIdAsync(id, userId);
            if (entry == null) return false;

            // Endast tillåtna fält uppdateras
            entry.Mood = dto.Mood;
            entry.Sleep = dto.Sleep;
            entry.Stress = dto.Stress;
            entry.Activity = dto.Activity;
            entry.Nutrition = dto.Nutrition;

            return await _repo.UpdateAsync(entry);
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

            await _repo.AddAsync(entry);

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
            return await _repo.DeleteAsync(id, userId);
        }

        public async Task<List<DailyWorkplaceStatsDto>> GetDailyStatsForWorkplaceAsync(int managerUserId)
        {
            var entries = await _context.Users
                .Include(u => u.Workplace)
                .Where(u => u.Id == managerUserId)
                .SelectMany(u => u.Workplace.Users.SelectMany(x => x.HealthEntries))
                .ToListAsync();

            var dailyStats = entries
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
                .ToList();

            return dailyStats;
        }

        public bool EntryExists(int id) => _repo.Exists(id);
    }
}
