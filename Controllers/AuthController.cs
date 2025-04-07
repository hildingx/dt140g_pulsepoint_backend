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

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            JwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            if (username == null)
            {
                return Unauthorized(new { message = "Invalid token or not logged in." });
            }

            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
                return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                user.Id,
                user.UserName,
                user.FirstName,
                user.LastName,
                user.WorkplaceId,
                Roles = roles
            });
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var user = new User
            {
                UserName = dto.Username,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                WorkplaceId = dto.WorkplaceId
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // Skicka inte vidare om registreringen inte lyckades
            await _userManager.AddToRoleAsync(user, "User");

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { message = "User registered successfully." });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);
            if (user == null) return BadRequest("Felaktigt användarnamn eller lösenord");

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded) return BadRequest("Felaktigt användarnamn eller lösenord");

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user, roles);

            return Ok(new{ token });
        }
    }
}
