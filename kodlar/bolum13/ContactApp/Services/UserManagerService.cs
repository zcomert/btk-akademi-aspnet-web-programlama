using ContactApp.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ContactApp.Services
{
    public class UserManagerService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManagerService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> AddUserToRoles(ApplicationUser user, IEnumerable<string> roles)
        {
            return await _userManager.AddToRolesAsync(user, roles);
        }

        public async Task<IdentityResult> CreateUser(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return IdentityResult
                    .Failed(new IdentityError { Description = "User not found." });
            }
            return await _userManager.DeleteAsync(user);
        }

        public async Task<ApplicationUser?> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<IList<string>> GetUserRoles(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<List<ApplicationUser>> GetUsers()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<IdentityResult> RemoveUserFromRoles(ApplicationUser user, IEnumerable<string> roles)
        {
            return await _userManager.RemoveFromRolesAsync(user, roles);
        }

        public async Task<IdentityResult> UpdateUser(ApplicationUser user)
        {
            var existingUser = await _userManager.FindByIdAsync(user.Id);
            if (existingUser == null)
            {
                return IdentityResult
                    .Failed(new IdentityError { Description = "Kullanıcı bulunamadı!" });
            }
            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;
            return await _userManager.UpdateAsync(existingUser);
        }

        public async Task<bool> UserExists(string userName)
        {
            return await _userManager.FindByNameAsync(userName) != null;
        }
    }
}
