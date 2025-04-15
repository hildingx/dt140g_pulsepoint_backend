using PulsePoint.Models;

namespace PulsePoint.Services
{
    /// <summary>
    /// Serviceklass för hantering av arbetsplatser.
    /// Ansvarar för affärslogik och vidarebefordrar anrop till arbetsplats-repositoryt.
    /// Används av WorkplacesController.
    /// </summary>
    public class WorkplaceService(IWorkplaceRepository repo) : IWorkplaceService
    {
        private readonly IWorkplaceRepository _repo = repo;

        /// <summary>
        /// Hämtar alla arbetsplatser.
        /// </summary>
        /// <returns>En lista med alla arbetsplatser i databasen.</returns>
        public Task<List<Workplace>> GetAllAsync()
        {
            return _repo.GetAllAsync();
        }

        /// <summary>
        /// Hämtar en enskild arbetsplats baserat på ID.
        /// </summary>
        /// <param name="id">ID för arbetsplatsen som ska hämtas.</param>
        /// <returns>Arbetsplatsen med matchande ID, eller null om den inte finns.</returns>
        public Task<Workplace?> GetByIdAsync(int id)
        {
            return _repo.GetByIdAsync(id);
        }

        /// <summary>
        /// Skapar en ny arbetsplats.
        /// </summary>
        /// <param name="workplace">Arbetsplatsobjekt som ska sparas.</param>
        /// <returns>Den skapade arbetsplatsen med tilldelat ID.</returns>
        public async Task<Workplace> CreateAsync(Workplace workplace)
        {
            // Kontrollera om arbetsplatsnamnet redan finns
            var all = await _repo.GetAllAsync();
            if (all.Any(w => w.Name.Trim().Equals(workplace.Name.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                return null; // eller kasta ett undantag beroende på stil
            }

            // Trimma namnet för att ta bort onödiga mellanslag
            workplace.Name = workplace.Name.Trim();
            return await _repo.AddAsync(workplace);
        }

        /// <summary>
        /// Uppdaterar en befintlig arbetsplats.
        /// </summary>
        /// <param name="workplace">Uppdaterade arbetsplatsdata (måste innehålla korrekt ID).</param>
        /// <returns>True om uppdatering lyckades, annars false.</returns>
        public Task<bool> UpdateAsync(Workplace workplace)
        {
            return _repo.UpdateAsync(workplace.Id, workplace);
        }

        /// <summary>
        /// Tar bort en arbetsplats baserat på ID.
        /// </summary>
        /// <param name="id">ID för arbetsplatsen som ska tas bort.</param>
        /// <returns>True om borttagning lyckades, annars false.</returns>
        public Task<bool> DeleteAsync(int id)
        {
            return _repo.DeleteAsync(id);
        }
    }
}
