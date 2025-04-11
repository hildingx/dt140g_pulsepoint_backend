using Microsoft.IdentityModel.Tokens;
using PulsePoint.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PulsePoint.Services
{
    /// <summary>
    /// Tjänst som skapar JWT-token för autentisering.
    /// Token innehåller användar-ID och tillhörande roller som claims.
    /// Används i AuthService vid inloggning.
    /// </summary>
    public class JwtService(IConfiguration config)
    {
        // Läser inställningar från appsettings eller .env (via IConfiguration)
        private readonly IConfiguration _config = config;

        /// <summary>
        /// Skapar en JWT-token för en specifik användare.
        /// </summary>
        /// <param name="user">Användaren som loggar in</param>
        /// <param name="roles">Roller som användaren tillhör</param>
        /// <returns>JWT-token som sträng</returns>
        public string GenerateToken(User user, IList<string> roles)
        {
            // Lista med claims som läggs in i token – här användarens ID
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            // Lägg till varje roll som en separat claim
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Skapa säkerhetsnyckel från konfigurerad hemlig nyckel
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            // Använd HMAC-SHA256 för att signera token
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Skapa token-objektet
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            // Serialisera token-objektet till en sträng
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
