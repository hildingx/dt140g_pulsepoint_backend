using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PulsePoint.Models;
using PulsePoint.Models.DTOs;
using PulsePoint.Services;
using System.Security.Claims;

namespace PulsePoint.Controllers
{
    /// <summary>
    /// API-kontroller för hantering av hälsoregistreringar (HealthEntries).
    /// Tillåter inloggade användare att skapa, hämta, uppdatera och ta bort sina egna registreringar.
    /// Manager-användare har även tillgång till aggregerad statistik för sin arbetsplats.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HealthEntriesController : ControllerBase
    {
        private readonly IHealthEntryService _healthEntryService;

        public HealthEntriesController(IHealthEntryService healthEntryService)
        {
            _healthEntryService = healthEntryService;
        }

        /// <summary>
        /// Hämtar alla health entries för den inloggade användaren.
        /// GET: api/HealthEntries
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HealthEntryResponseDto>>> GetHealthEntries()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var entries = await _healthEntryService.GetEntriesForUserAsync(userId);
            return Ok(entries);
        }

        /// <summary>
        /// Hämtar en specifik health entry baserat på ID, för aktuell användare.
        /// GET: api/HealthEntries/{id}
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<HealthEntry>> GetHealthEntry(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var entry = await _healthEntryService.GetEntryByIdAsync(id, userId);
            if (entry == null) return NotFound();
            return Ok(entry);
        }

        /// <summary>
        /// Uppdaterar en befintlig health entry för användaren.
        /// PUT: api/HealthEntries/{id}
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHealthEntry(int id, HealthEntryRequestDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var success = await _healthEntryService.UpdateEntryAsync(id, userId, dto);

            if (!success)
                return NotFound(); // Antingen entry saknas eller ej användarens

            return NoContent();
        }

        /// <summary>
        /// Skapar en ny health entry för användaren.
        /// POST: api/HealthEntries
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> PostHealthEntry(HealthEntryRequestDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var response = await _healthEntryService.CreateEntryAsync(userId, dto);

            return CreatedAtAction(nameof(GetHealthEntry), new { id = response.Id }, response);
        }

        /// <summary>
        /// Tar bort en health entry (om den tillhör användaren).
        /// DELETE: api/HealthEntries/{id}
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHealthEntry(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var deleted = await _healthEntryService.DeleteEntryAsync(id, userId);

            if (!deleted)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Hämtar aggregerad statistik per dag för alla användare på samma workplace som den inloggade managern.
        /// GET: api/HealthEntries/stats/daily
        /// </summary>
        [HttpGet("stats/daily")]
        [Authorize(Roles = "manager")]
        public async Task<IActionResult> GetDailyStatsForWorkplace()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim is null || !int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Invalid token or user ID." });
            }

            var stats = await _healthEntryService.GetDailyStatsForWorkplaceAsync(userId);

            if (stats.Count == 0)
                return NotFound("No data found or invalid manager.");

            return Ok(stats);
        }
    }
}
