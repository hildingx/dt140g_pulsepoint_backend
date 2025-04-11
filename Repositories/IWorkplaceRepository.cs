using PulsePoint.Models;

namespace PulsePoint.Services
{
    /// <summary>
    /// Interface för WorkplaceRepository – definierar metoder för CRUD-operationer mot arbetsplatser.
    /// Implementeras av WorkplaceRepository och används av WorkplaceService.
    /// </summary>
    public interface IWorkplaceRepository
    {
        /// <summary>
        /// Hämtar en lista med alla arbetsplatser.
        /// </summary>
        /// <returns>Lista med alla workplaces</returns>
        Task<List<Workplace>> GetAllAsync();

        /// <summary>
        /// Hämtar en specifik arbetsplats baserat på ID.
        /// </summary>
        /// <param name="id">ID för arbetsplatsen</param>
        /// <returns>Workplace eller null om den inte finns</returns>
        Task<Workplace?> GetByIdAsync(int id);

        /// <summary>
        /// Skapar en ny arbetsplats.
        /// </summary>
        /// <param name="workplace">Arbetsplats att lägga till</param>
        /// <returns>Den skapade arbetsplatsen</returns>
        Task<Workplace> AddAsync(Workplace workplace);

        /// <summary>
        /// Uppdaterar en befintlig arbetsplats.
        /// </summary>
        /// <param name="id">ID för arbetsplatsen som ska uppdateras</param>
        /// <param name="updated">Uppdaterade värden</param>
        /// <returns>True om uppdateringen lyckades, annars false</returns>
        Task<bool> UpdateAsync(int id, Workplace updated);

        /// <summary>
        /// Tar bort en arbetsplats baserat på ID.
        /// </summary>
        /// <param name="id">ID för arbetsplatsen</param>
        /// <returns>True om borttagningen lyckades, annars false</returns>
        Task<bool> DeleteAsync(int id);
    }
}
