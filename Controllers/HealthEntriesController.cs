using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PulsePoint.Data;
using PulsePoint.Models;
using PulsePoint.Models.DTOs;
using PulsePoint.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PulsePoint.Controllers
{
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

        // GET: api/HealthEntries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HealthEntryResponseDto>>> GetHealthEntries()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var entries = await _healthEntryService.GetEntriesForUserAsync(userId);
            return Ok(entries);
        }

        // GET: api/HealthEntries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HealthEntry>> GetHealthEntry(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var entry = await _healthEntryService.GetEntryByIdAsync(id, userId);
            if (entry == null) return NotFound();
            return Ok(entry);
        }

        // PUT: api/HealthEntries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHealthEntry(int id, HealthEntryRequestDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var success = await _healthEntryService.UpdateEntryAsync(id, userId, dto);

            if (!success)
                return NotFound(); // Antingen entry saknas, inte användarens, eller fel vid update

            return NoContent();
        }

        // POST: api/HealthEntries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostHealthEntry(HealthEntryRequestDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var response = await _healthEntryService.CreateEntryAsync(userId, dto);

            return CreatedAtAction(nameof(GetHealthEntry), new { id = response.Id }, response);
        }



        // DELETE: api/HealthEntries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHealthEntry(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var deleted = await _healthEntryService.DeleteEntryAsync(id, userId);

            if (!deleted)
                return NotFound();

            return NoContent();
        }


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
