using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PulsePoint.Models;
using PulsePoint.Models.DTOs;
using PulsePoint.Services;
using System.Security.Claims;

namespace PulsePoint.Controllers
{
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

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var (success, errors) = await _authService.RegisterAsync(dto);

            if (!success)
                return BadRequest(errors);

            return Ok(new { message = "User registered successfully." });
        }

        // POST: api/auth/login
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
