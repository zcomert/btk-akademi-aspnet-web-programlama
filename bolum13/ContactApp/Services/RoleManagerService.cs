using ContactApp.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ContactApp.Services;

public class RoleManagerService : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    public RoleManagerService(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<IdentityResult> CreateRole(string roleName)
    {
        var role = new ApplicationRole { Name = roleName };
        return await _roleManager.CreateAsync(role);
    }

    public async Task<IdentityResult> DeleteRole(string id)
    {
        var role = await _roleManager.FindByIdAsync(id);
        if (role == null) 
        {
            return IdentityResult
                .Failed(new IdentityError { Description = "Role bulunamadı." });
        }
        return await _roleManager.DeleteAsync(role);
    }

    public async Task<ApplicationRole?> GetRoleById(string id)
    {
        return await _roleManager.FindByIdAsync(id);
    }

    public async Task<List<ApplicationRole>> GetRoles()
    {
        return await _roleManager.Roles.ToListAsync();
    }

    public async Task<bool> RoleExists(string roleName)
    {
        return await _roleManager.RoleExistsAsync(roleName);
    }

    public async Task<IdentityResult> UpdateRole(ApplicationRole role)
    {
        var existingRole = await _roleManager.FindByIdAsync(role.Id);
        if (existingRole == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "Role bulunamadı." });
        }
        existingRole.Name = role.Name;
        return await _roleManager.UpdateAsync(existingRole);
    }
}
