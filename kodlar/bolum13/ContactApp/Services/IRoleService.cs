using ContactApp.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace ContactApp.Services;

public interface IRoleService
{
    Task<List<ApplicationRole>> GetRoles();
    Task<ApplicationRole?> GetRoleById(string id);
    Task<IdentityResult> CreateRole(string roleName);
    Task<IdentityResult> UpdateRole(ApplicationRole role);
    Task<IdentityResult> DeleteRole(string id);
    Task<bool> RoleExists(string roleName);
}
