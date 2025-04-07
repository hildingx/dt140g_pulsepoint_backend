﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PulsePoint.Data;
using PulsePoint.Models;
using PulsePoint.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PulsePoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthEntriesController : ControllerBase
    {
        private readonly PulsePointDbContext _context;

        public HealthEntriesController(PulsePointDbContext context)
        {
            _context = context;
        }

        // GET: api/HealthEntries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HealthEntry>>> GetHealthEntries()
        {
            return await _context.HealthEntries.ToListAsync();
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
        public async Task<IActionResult> PostHealthEntry(HealthEntryDto dto)
        {
            var entry = new HealthEntry
            {
                Mood = dto.Mood,
                Sleep = dto.Sleep,
                Stress = dto.Stress,
                Activity = dto.Activity,
                Nutrition = dto.Nutrition,
                Date = DateTime.Today,
                UserId = dto.UserId
            };

            _context.HealthEntries.Add(entry);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHealthEntry", new { id = entry.Id }, entry);
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
