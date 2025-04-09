using PulsePoint.Models;
using PulsePoint.Models.DTOs;
using System.Security.Claims;

namespace PulsePoint.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly JwtService _jwtService;

        public AuthService(IUserRepository userRepo, JwtService jwtService)
        {
            _userRepo = userRepo;
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

            var result = await _userRepo.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return (false, result.Errors.Select(e => e.Description));

            await _userRepo.AddToRoleAsync(user, "User");

            return (true, Enumerable.Empty<string>());
        }

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
