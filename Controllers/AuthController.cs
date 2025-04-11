using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PulsePoint.Models;
using PulsePoint.Models.DTOs;
using PulsePoint.Services;
using System.Security.Claims;

namespace PulsePoint.Controllers
{
    /// <summary>
    /// Controller för autentisering. Hanterar registrering, inloggning och hämtning av information om inloggad användare.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtService _jwtService;
        private readonly IAuthService _authService;

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            JwtService jwtService,
            IAuthService authService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _authService = authService;
        }

        /// <summary>
        /// Registrerar en ny användare.
        /// </summary>
        /// <param name="dto">Data från frontend: användarnamn, lösenord, förnamn, efternamn, arbetsplats-ID.</param>
        /// <returns>200 OK om lyckad registrering, annars 400 med felmeddelanden.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var (success, errors) = await _authService.RegisterAsync(dto);

            if (!success)
                return BadRequest(errors);

            return Ok(new { message = "User registered successfully." });
        }

        /// <summary>
        /// Loggar in en användare och returnerar en JWT-token.
        /// </summary>
        /// <param name="dto">Användarnamn och lösenord.</param>
        /// <returns>JWT-token och användarinformation om inloggning lyckas, annars 400.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var (token, username, roles) = await _authService.LoginAsync(dto);

            if (token is null)
                return BadRequest("Felaktigt användarnamn eller lösenord");

            return Ok(new
            {
                token,
                username,
                roles
            });
        }

        /// <summary>
        /// Hämtar information om den inloggade användaren baserat på token.
        /// </summary>
        /// <returns>Användarens ID, användarnamn, namn, arbetsplats och roller.</returns>
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var userInfo = await _authService.GetCurrentUserAsync(User);

            if (userInfo == null)
                return Unauthorized(new { message = "Invalid token or not logged in." });

            return Ok(userInfo);
        }
    }
}
