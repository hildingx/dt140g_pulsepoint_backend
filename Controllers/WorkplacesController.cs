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
    /// Controller för hantering av arbetsplatser (Workplace).
    /// Exponerar endpoints för att hämta, skapa, uppdatera och ta bort arbetsplatser.
    [Route("api/[controller]")]
    [ApiController]
    public class WorkplacesController(IWorkplaceService workplaceService) : ControllerBase
    {
        private readonly IWorkplaceService _workplaceService = workplaceService;

        /// <summary>
        /// Hämtar en lista med alla arbetsplatser i systemet.
        /// Används t.ex. för att visa tillgängliga arbetsplatser i ett registreringsformulär.
        /// </summary>
        /// <returns>En lista med arbetsplatser.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Workplace>>> GetWorkplaces()
        {
            var workplaces = await _workplaceService.GetAllAsync();
            return Ok(workplaces);
        }

        /// <summary>
        /// Skapar en ny arbetsplats i systemet.
        /// Endast tillgänglig för användare med rollen "admin".
        /// </summary>
        /// <param name="workplace">Det arbetsplatsobjekt som ska skapas.</param>
        /// <returns>Den skapade arbetsplatsen med tilldelat ID.</returns>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Workplace>> PostWorkplace(Workplace workplace)
        {
            var created = await _workplaceService.CreateAsync(workplace);
            return CreatedAtAction(nameof(GetWorkplaces), new { id = created.Id }, created);
        }

        /// <summary>
        /// Uppdaterar en befintlig arbetsplats.
        /// Endast tillgänglig för användare med rollen "admin".
        /// </summary>
        /// <param name="id">ID för den arbetsplats som ska uppdateras.</param>
        /// <param name="workplace">Uppdaterade arbetsplatsdata.</param>
        /// <returns>NoContent om uppdatering lyckas, annars NotFound om ID inte hittas.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutWorkplace(int id, Workplace workplace)
        {
            var updated = await _workplaceService.UpdateAsync(workplace);
            if (!updated) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Tar bort en arbetsplats baserat på ID.
        /// Endast tillgänglig för användare med rollen "admin".
        /// </summary>
        /// <param name="id">ID för den arbetsplats som ska tas bort.</param>
        /// <returns>NoContent om borttagning lyckas, annars NotFound om ID inte hittas.</returns>
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
