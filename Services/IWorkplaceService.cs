using PulsePoint.Models;

namespace PulsePoint.Services
{
    /// <summary>
    /// Interface för tjänst som hanterar arbetsplatser (Workplaces).
    /// Används av WorkplacesController för att anropa affärslogik.
    /// </summary>
    public interface IWorkplaceService
    {
        /// <summary>
        /// Hämtar en lista med alla arbetsplatser.
        /// </summary>
        /// <returns>Lista med Workplace-objekt</returns>
        Task<List<Workplace>> GetAllAsync();

        /// <summary>
        /// Hämtar en specifik arbetsplats baserat på ID.
        /// </summary>
        /// <param name="id">ID för arbetsplats</param>
        /// <returns>Workplace-objekt eller null om det inte hittades</returns>
        Task<Workplace?> GetByIdAsync(int id);

        /// <summary>
        /// Skapar en ny arbetsplats.
        /// </summary>
        /// <param name="workplace">Arbetsplats att skapa</param>
        /// <returns>Den skapade arbetsplatsen</returns>
        Task<Workplace> CreateAsync(Workplace workplace);

        /// <summary>
        /// Uppdaterar en befintlig arbetsplats.
        /// </summary>
        /// <param name="workplace">Arbetsplats med uppdaterade värden</param>
        /// <returns>True om uppdateringen lyckades</returns>
        Task<bool> UpdateAsync(Workplace workplace);

        /// <summary>
        /// Tar bort en arbetsplats baserat på ID.
        /// </summary>
        /// <param name="id">ID för arbetsplats</param>
        /// <returns>True om borttagningen lyckades</returns>
        Task<bool> DeleteAsync(int id);
    }
}
