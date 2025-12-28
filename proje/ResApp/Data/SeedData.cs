using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ResApp.Models;
using ResApp.Options;

namespace ResApp.Data;

public static class SeedData
{
    public static async Task EnsureSeededAsync(IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var seedUserOptions = scope.ServiceProvider.GetRequiredService<IOptions<SeedUserOptions>>().Value;

        var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            await context.Database.MigrateAsync();
        }

        if (await userManager.Users.AnyAsync())
        {
            return;
        }

        foreach (var userCredential in seedUserOptions.Users)
        {
            var role = userCredential.Role;

            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }

            var user = new User
            {
                UserName = userCredential.UserName,
                Email = userCredential.Email
            };

            var result = await userManager.CreateAsync(user, userCredential.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);

                var profile = new UserProfile
                {
                    UserId = user.Id,
                    FullName = userCredential.FullName,
                    Email = userCredential.Email
                };
                context.UserProfiles.Add(profile);
            }
        }
        
        await context.SaveChangesAsync();
    }
}
