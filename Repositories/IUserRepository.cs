using Microsoft.AspNetCore.Identity;
using PulsePoint.Models;

namespace PulsePoint.Services
{
    /// <summary>
    /// Interface för användarhantering.
    /// Beskriver vilka metoder som måste implementeras i UserRepository.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Hämta en användare baserat på användarnamn.
        /// </summary>
        Task<User?> FindByUsernameAsync(string username);

        /// <summary>
        /// Hämta en användare baserat på användarens ID.
        /// </summary>
        Task<User?> FindByIdAsync(string id);

        /// <summary>
        /// Skapa en ny användare med lösenord.
        /// </summary>
        Task<IdentityResult> CreateAsync(User user, string password);

        /// <summary>
        /// Kontrollera om angivet lösenord är korrekt för användaren.
        /// </summary>
        Task<SignInResult> CheckPasswordAsync(User user, string password);

        /// <summary>
        /// Hämta alla roller för en användare.
        /// </summary>
        Task<IList<string>> GetRolesAsync(User user);

        /// <summary>
        /// Tilldela en roll till en användare.
        /// </summary>
        Task AddToRoleAsync(User user, string role);

        /// <summary>
        /// Hämta alla hälsoregistreringar som är kopplade till en viss arbetsplats.
        /// </summary>
        Task<List<HealthEntry>> GetHealthEntriesForManagerWorkplaceAsync(int managerUserId);

        /// <summary>
        /// Kontrollerar om en angiven arbetsplats finns i databasen.
        /// </summary>
        /// <param name="workplaceId">ID för arbetsplatsen</param>
        /// <returns>True om arbetsplatsen finns, annars false</returns>
        Task<bool> WorkplaceExistsAsync(int workplaceId);

    }
}
