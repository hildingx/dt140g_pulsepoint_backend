using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PulsePoint.Models;
using PulsePoint.Models.DTOs;
using System.Security.Claims;

namespace PulsePoint.Services
{
    /// <summary>
    /// Serviceklass för autentisering och användarhantering.
    /// Implementerar IAuthService och kallas från AuthController.
    /// Kommunicerar med databasen via IUserRepository och genererar JWT med JwtService.
    /// </summary>
    public class AuthService(IUserRepository userRepo, JwtService jwtService) : IAuthService
    {
        private readonly IUserRepository _userRepo = userRepo;
        private readonly JwtService _jwtService = jwtService;

        /// <summary>
        /// Registrerar en ny användare i systemet.
        /// </summary>
        /// <param name="dto">RegisterDto med användarinfo</param>
        /// <returns>True om lyckad registrering, annars en lista med fel</returns>
        public async Task<(bool Success, IEnumerable<string> Errors)> RegisterAsync(RegisterDto dto)
        {
            // Kolla om användarnamnet redan är taget
            var existingUser = await _userRepo.FindByUsernameAsync(dto.Username);
            if (existingUser != null)
            {
                return (false, new[] { "Användarnamnet är redan taget." });
            }

            // Kolla om arbetsplats-id finns
            var workplaceExists = await _userRepo.WorkplaceExistsAsync(dto.WorkplaceId);
            if (!workplaceExists)
            {
                return (false, new List<string> { "Ogiltigt WorkplaceId." });
            }

            // Trimma inputdata
            dto.Username = dto.Username.Trim();
            dto.FirstName = dto.FirstName?.Trim();
            dto.LastName = dto.LastName?.Trim();

            // Skapa ny User-objekt baserat på data från klienten
            var user = new User
            {
                UserName = dto.Username,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                WorkplaceId = dto.WorkplaceId
            };

            // Skapa användaren i databasen (inkl lösenordshashning)
            var result = await _userRepo.CreateAsync(user, dto.Password);

            // Om något gick fel – returnera felmeddelanden
            if (!result.Succeeded)
                return (false, result.Errors.Select(e => e.Description));

            // Tilldela användarrollen "User"
            await _userRepo.AddToRoleAsync(user, "User");

            return (true, Enumerable.Empty<string>());
        }

        /// <summary>
        /// Loggar in en användare och returnerar en JWT-token.
        /// </summary>
        /// <param name="dto">LoginDto med användarnamn och lösenord</param>
        /// <returns>JWT-token + användarnamn och roller om inloggning lyckas, annars null</returns>
        public async Task<(string? Token, string? Username, IList<string> Roles)> LoginAsync(LoginDto dto)
        {
            var user = await _userRepo.FindByUsernameAsync(dto.Username);
            if (user == null) return (null, null, new List<string>());

            var result = await _userRepo.CheckPasswordAsync(user, dto.Password);
            if (!result.Succeeded) return (null, null, new List<string>());

            var roles = await _userRepo.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user, roles);

            return (token, user.UserName, roles);
        }

        /// <summary>
        /// Hämtar information om den nuvarande inloggade användaren (via JWT).
        /// </summary>
        /// <param name="userClaims">Användarens ClaimsPrincipal (från token)</param>
        /// <returns>Objekt med användarinfo (eller null om ogiltig token)</returns>
        public async Task<object?> GetCurrentUserAsync(ClaimsPrincipal userClaims)
        {
            var userIdClaim = userClaims.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return null;

            var user = await _userRepo.FindByIdAsync(userIdClaim);
            if (user == null) return null;

            var roles = await _userRepo.GetRolesAsync(user);

            return new
            {
                user.Id,
                user.UserName,
                user.FirstName,
                user.LastName,
                user.WorkplaceId,
                Roles = roles
            };
        }
    }
}
