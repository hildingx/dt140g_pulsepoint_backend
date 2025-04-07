using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PulsePoint.Data;
using PulsePoint.Models;
using PulsePoint.Models.DTOs;
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
        private readonly PulsePointDbContext _context;

        public HealthEntriesController(PulsePointDbContext context)
        {
            _context = context;
        }

        // GET: api/HealthEntries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HealthEntryResponseDto>>> GetHealthEntries()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var entries = await _context.HealthEntries
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

            return Ok(entries);
        }

        // GET: api/HealthEntries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HealthEntry>> GetHealthEntry(int id)
        {
            var healthEntry = await _context.HealthEntries.FindAsync(id);

            if (healthEntry == null)
            {
                return NotFound();
            }

            return healthEntry;
        }

        // PUT: api/HealthEntries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHealthEntry(int id, HealthEntry healthEntry)
        {
            if (id != healthEntry.Id)
            {
                return BadRequest();
            }

            _context.Entry(healthEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HealthEntryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/HealthEntries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostHealthEntry(HealthEntryCreateDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

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

            var response = new HealthEntryResponseDto
            {
                Id = entry.Id,
                Mood = entry.Mood,
                Sleep = entry.Sleep,
                Stress = entry.Stress,
                Activity = entry.Activity,
                Nutrition = entry.Nutrition,
                Date = entry.Date
            };

            return CreatedAtAction(nameof(GetHealthEntry), new { id = entry.Id }, response);
        }


        // DELETE: api/HealthEntries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHealthEntry(int id)
        {
            var healthEntry = await _context.HealthEntries.FindAsync(id);
            if (healthEntry == null)
            {
                return NotFound();
            }

            _context.HealthEntries.Remove(healthEntry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HealthEntryExists(int id)
        {
            return _context.HealthEntries.Any(e => e.Id == id);
        }
    }
}
