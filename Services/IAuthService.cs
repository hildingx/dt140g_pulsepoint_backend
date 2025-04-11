using PulsePoint.Models.DTOs;
using System.Security.Claims;

namespace PulsePoint.Services
{
    /// <summary>
    /// Definierar kontraktet för autentiseringstjänsten.
    /// Implementeras av AuthService och används i AuthController.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Registrerar en ny användare med angivna uppgifter.
        /// </summary>
        /// <param name="dto">RegisterDto med användarnamn, lösenord, namn och workplaceId</param>
        /// <returns>Tuple med bool för om registreringen lyckades och ev. felmeddelanden</returns>
        Task<(bool Success, IEnumerable<string> Errors)> RegisterAsync(RegisterDto dto);

        /// <summary>
        /// Loggar in en användare och returnerar en JWT-token vid korrekt inloggning.
        /// </summary>
        /// <param name="dto">LoginDto med användarnamn och lösenord</param>
        /// <returns>Tuple med token, användarnamn och tillhörande roller (eller null vid misslyckande)</returns>
        Task<(string? Token, string? Username, IList<string> Roles)> LoginAsync(LoginDto dto);

        /// <summary>
        /// Hämtar information om nuvarande användare utifrån ClaimsPrincipal (JWT-token).
        /// </summary>
        /// <param name="user">ClaimsPrincipal från den inloggade användaren</param>
        /// <returns>Objekt med användardata (eller null om ogiltig token)</returns>
        Task<object?> GetCurrentUserAsync(ClaimsPrincipal user);
    }
}
