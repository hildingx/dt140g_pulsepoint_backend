using Microsoft.AspNetCore.Identity;
using PulsePoint.Models;

namespace PulsePoint.Services
{
    public interface IUserRepository
    {
        Task<User?> FindByUsernameAsync(string username);
        Task<User?> FindByIdAsync(string id);
        Task<IdentityResult> CreateAsync(User user, string password);
        Task<SignInResult> CheckPasswordAsync(User user, string password);
        Task<IList<string>> GetRolesAsync(User user);
        Task AddToRoleAsync(User user, string role);
    }
}
