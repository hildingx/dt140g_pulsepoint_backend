using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PulsePoint.Data;
using PulsePoint.Models;
using PulsePoint.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PulsePoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkplacesController : ControllerBase
    {
        private readonly IWorkplaceService _workplaceService;

        public WorkplacesController(IWorkplaceService workplaceService)
        {
            _workplaceService = workplaceService;
        }

        // GET: api/Workplaces
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Workplace>>> GetWorkplaces()
        {
            var workplaces = await _workplaceService.GetAllAsync();
            return Ok(workplaces);
        }

        // POST: api/Workplaces
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Workplace>> PostWorkplace(Workplace workplace)
        {
            var created = await _workplaceService.CreateAsync(workplace);
            return CreatedAtAction(nameof(GetWorkplaces), new { id = created.Id }, created);
        }

        // PUT: api/Workplaces/5
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutWorkplace(int id, Workplace workplace)
        {
            var updated = await _workplaceService.UpdateAsync(workplace);
            if (!updated) return NotFound();
            return NoContent();
        }

        // DELETE: api/Workplaces/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteWorkplace(int id)
        {
            var deleted = await _workplaceService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
