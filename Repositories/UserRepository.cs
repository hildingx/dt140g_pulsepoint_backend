using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PulsePoint.Data;
using PulsePoint.Models;

namespace PulsePoint.Services
{
    /// <summary>
    /// Repository för användarhantering med Identity.
    /// Kapslar in alla anrop till UserManager och SignInManager.
    /// Används av AuthService för registrering, inloggning och rollhantering.
    /// </summary>
    public class UserRepository(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        PulsePointDbContext context) : IUserRepository
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly PulsePointDbContext _context = context;

        /// <summary>
        /// Hämta användare baserat på användarnamn.
        /// </summary>
        public Task<User?> FindByUsernameAsync(string username)
        {
            return _userManager.FindByNameAsync(username);
        }

        /// <summary>
        /// Hämta användare baserat på ID (från JWT-token).
        /// </summary>
        public Task<User?> FindByIdAsync(string id)
        {
            return _userManager.FindByIdAsync(id);
        }

        /// <summary>
        /// Skapa en ny användare med angivet lösenord.
        /// </summary>
        public Task<IdentityResult> CreateAsync(User user, string password)
        {
            return _userManager.CreateAsync(user, password);
        }

        /// <summary>
        /// Tilldela en roll till en användare (ex. "User", "Manager", "Admin").
        /// </summary>
        public Task AddToRoleAsync(User user, string role)
        {
            return _userManager.AddToRoleAsync(user, role);
        }

        /// <summary>
        /// Hämta alla roller som en användare tillhör.
        /// </summary>
        public Task<IList<string>> GetRolesAsync(User user)
        {
            return _userManager.GetRolesAsync(user);
        }

        /// <summary>
        /// Kontrollera att lösenordet är korrekt vid inloggning.
        /// </summary>
        public Task<SignInResult> CheckPasswordAsync(User user, string password)
        {
            return _signInManager.CheckPasswordSignInAsync(user, password, false);
        }

        /// <summary>
        /// Hämtar alla hälsoinlägg för den arbetsplats som en viss manager ansvarar för.
        /// Manager identifieras via sitt användar-ID.
        /// </summary>
        /// <param name="managerUserId">ID för manager-användaren</param>
        /// <returns>Lista med HealthEntry-objekt för alla användare på managerns arbetsplats</returns>
        public async Task<List<HealthEntry>> GetHealthEntriesForManagerWorkplaceAsync(int managerUserId)
        {
            return await _context.Users
                .Include(u => u.Workplace)
                    .ThenInclude(w => w.Users)
                        .ThenInclude(u => u.HealthEntries)
                .Where(u => u.Id == managerUserId)
                .SelectMany(u => u.Workplace.Users.SelectMany(x => x.HealthEntries))
                .ToListAsync();
        }

        /// <summary>
        /// Kontrollerar om en angiven arbetsplats (WorkplaceId) existerar i databasen.
        /// </summary>
        /// <param name="workplaceId">ID för arbetsplatsen</param>
        /// <returns>True om arbetsplatsen finns, annars false</returns>
        public async Task<bool> WorkplaceExistsAsync(int workplaceId)
        {
            return await _context.Workplaces.AnyAsync(w => w.Id == workplaceId);
        }
    }
}
