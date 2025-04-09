using Microsoft.AspNetCore.Identity;
using PulsePoint.Models;
using PulsePoint.Models.DTOs;
using System.Security.Claims;

namespace PulsePoint.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtService _jwtService;

        public AuthService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            JwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }

        public async Task<(bool Success, IEnumerable<string> Errors)> RegisterAsync(RegisterDto dto)
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
                return (false, result.Errors.Select(e => e.Description));

            await _userManager.AddToRoleAsync(user, "User");

            return (true, Enumerable.Empty<string>());
        }

        public async Task<(string? Token, string? Username, IList<string> Roles)> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);
            if (user == null) return (null, null, new List<string>());

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded) return (null, null, new List<string>());

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateToken(user, roles);

            return (token, user.UserName, roles);
        }

        public async Task<object?> GetCurrentUserAsync(ClaimsPrincipal userClaims)
        {
            var userIdClaim = userClaims.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return null;

            var user = await _userManager.FindByIdAsync(userIdClaim);
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);

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
