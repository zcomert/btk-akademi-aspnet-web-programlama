using ContactApp.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace ContactApp.Services;

public interface IUserService
{
    Task<List<ApplicationUser>> GetUsers();
    Task<ApplicationUser?> GetUserById(string id);
    Task<IdentityResult> CreateUser(ApplicationUser user, string password);
    Task<IdentityResult> UpdateUser(ApplicationUser user);
    Task<IdentityResult> DeleteUser(string id);
    Task<IList<string>> GetUserRoles(ApplicationUser user);
    Task<IdentityResult> AddUserToRoles(ApplicationUser user, IEnumerable<string> roles);
    Task<IdentityResult> RemoveUserFromRoles(ApplicationUser user, IEnumerable<string> roles);
    Task<bool> UserExists(string userName);
}