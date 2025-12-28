using LibApp.Models.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibApp.Data;

public static class AdminUserSeeder
{
    public static async Task SeedAdminUserAsync(AppDbContext context,
        AdminUserOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Username) 
            || string.IsNullOrWhiteSpace(options.Password))
        {
            return;
        }

        var roleName = string.IsNullOrWhiteSpace(options.Role) 
            ? "Administrator" 
            : options.Role;

        var role = await context
            .Roles
            .FirstOrDefaultAsync(r => r.Name == roleName);

        if (role is null)
        {
            role = new IdentityRole
            {
                Name = roleName,
                NormalizedName = roleName.ToUpperInvariant()
            };
            await context.Roles.AddAsync(role);
        }

        var adminUser = await context
            .Users
            .FirstOrDefaultAsync(u => u.UserName == options.Username);

        if (adminUser is null)
        {
            adminUser = new IdentityUser
            {
                UserName = options.Username,
                NormalizedUserName = options.Username.ToUpperInvariant(),
                EmailConfirmed = true
            };

            var hasher = new PasswordHasher<IdentityUser>();
            adminUser.PasswordHash = hasher.HashPassword(adminUser, options.Password);

            await context.Users.AddAsync(adminUser);
        }

        var userRoleExists = await context.UserRoles
            .AnyAsync(ur => ur.UserId == adminUser.Id && ur.RoleId == role.Id);

        if (!userRoleExists)
        {
            await context.UserRoles.AddAsync(new IdentityUserRole<string>
            {
                UserId = adminUser.Id,
                RoleId = role.Id
            });
        }

        await context.SaveChangesAsync();
    }
}
