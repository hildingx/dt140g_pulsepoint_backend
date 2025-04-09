using Microsoft.AspNetCore.Identity;
using PulsePoint.Models;

namespace PulsePoint.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserRepository(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public Task<User?> FindByUsernameAsync(string username)
        {
            return _userManager.FindByNameAsync(username);
        }

        public Task<User?> FindByIdAsync(string id)
        {
            return _userManager.FindByIdAsync(id);
        }

        public Task<IdentityResult> CreateAsync(User user, string password)
        {
            return _userManager.CreateAsync(user, password);
        }

        public Task AddToRoleAsync(User user, string role)
        {
            return _userManager.AddToRoleAsync(user, role);
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            return _userManager.GetRolesAsync(user);
        }

        public Task<SignInResult> CheckPasswordAsync(User user, string password)
        {
            return _signInManager.CheckPasswordSignInAsync(user, password, false);
        }
    }
}
