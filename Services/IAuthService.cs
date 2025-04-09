using PulsePoint.Models.DTOs;
using System.Security.Claims;

namespace PulsePoint.Services
{
    public interface IAuthService
    {
        Task<(bool Success, IEnumerable<string> Errors)> RegisterAsync(RegisterDto dto);
        Task<(string? Token, string? Username, IList<string> Roles)> LoginAsync(LoginDto dto);
        Task<object?> GetCurrentUserAsync(ClaimsPrincipal user);
    }
}