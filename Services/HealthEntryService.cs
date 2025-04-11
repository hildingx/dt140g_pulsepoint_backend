using Microsoft.EntityFrameworkCore;
using PulsePoint.Data;
using PulsePoint.Models;
using PulsePoint.Models.DTOs;
using PulsePoint.Repositories;

namespace PulsePoint.Services
{
    /// <summary>
    /// Serviceklass som hanterar logik relaterad till användarens hälsoregistreringar.
    /// Använder HealthEntryRepository för databasanrop och UserRepository för managerspecifik logik.
    /// </summary>
    public class HealthEntryService(
        IHealthEntryRepository repo,
        IUserRepository userRepository) : IHealthEntryService
    {
        private readonly IHealthEntryRepository _repo = repo;
        private readonly IUserRepository _userRepository = userRepository;

        /// <summary>
        /// Hämtar alla health entries för en specifik användare.
        /// </summary>
        /// <param name="userId">ID för den användare vars entries ska hämtas.</param>
        /// <returns>En lista med DTO-objekt för varje entry.</returns>
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

        /// <summary>
        /// Hämtar en specifik health entry för en viss användare.
        /// </summary>
        /// <param name="id">Entry-ID</param>
        /// <param name="userId">Användar-ID</param>
        /// <returns>DTO för entry om den finns, annars null.</returns>
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

        /// <summary>
        /// Uppdaterar en befintlig entry för en specifik användare.
        /// </summary>
        /// <param name="id">Entry-ID</param>
        /// <param name="userId">Användar-ID</param>
        /// <param name="dto">DTO med uppdaterade värden</param>
        /// <returns>True om uppdatering lyckades, annars false.</returns>
        public async Task<bool> UpdateEntryAsync(int id, int userId, HealthEntryRequestDto dto)
        {
            var entry = await _repo.GetByIdAsync(id, userId);
            if (entry == null) return false;

            entry.Mood = dto.Mood;
            entry.Sleep = dto.Sleep;
            entry.Stress = dto.Stress;
            entry.Activity = dto.Activity;
            entry.Nutrition = dto.Nutrition;

            return await _repo.UpdateAsync(entry);
        }

        /// <summary>
        /// Skapar en ny entry för en användare.
        /// </summary>
        /// <param name="userId">Användarens ID</param>
        /// <param name="dto">DTO med värden</param>
        /// <returns>DTO med sparad entry</returns>
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

        /// <summary>
        /// Tar bort en specifik health entry för en användare.
        /// </summary>
        /// <param name="id">Entry-ID</param>
        /// <param name="userId">Användar-ID</param>
        /// <returns>True om borttagning lyckades, annars false.</returns>
        public async Task<bool> DeleteEntryAsync(int id, int userId)
        {
            return await _repo.DeleteAsync(id, userId);
        }

        /// <summary>
        /// Hämtar aggregerad statistik dag-för-dag för alla användare i managerns arbetsplats.
        /// </summary>
        /// <param name="managerUserId">Managerns användar-ID</param>
        /// <returns>Lista av DTOs med genomsnittsvärden per dag.</returns>
        public async Task<List<DailyWorkplaceStatsDto>> GetDailyStatsForWorkplaceAsync(int managerUserId)
        {
            var entries = await _userRepository.GetHealthEntriesForManagerWorkplaceAsync(managerUserId);

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

        /// <summary>
        /// Kollar om en health entry med angivet ID existerar.
        /// </summary>
        /// <param name="id">Entry-ID</param>
        /// <returns>True om entry finns, annars false.</returns>
        public bool EntryExists(int id) => _repo.Exists(id);
    }
}
