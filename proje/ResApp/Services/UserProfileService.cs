using Microsoft.EntityFrameworkCore;
using ResApp.Data;
using ResApp.Models;

namespace ResApp.Services;

public class UserProfileService : IUserProfileService
{
    private readonly AppDbContext _context;

    public UserProfileService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserProfile?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _context.UserProfiles.AsNoTracking().FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);
    }

    public async Task<UserProfile> EnsureProfileAsync(string userId, CancellationToken cancellationToken = default)
    {
        var profile = await _context.UserProfiles.FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);
        if (profile is not null)
        {
            return profile;
        }

        profile = new UserProfile
        {
            UserId = userId,
            FullName = "İsimsiz Kullanıcı",
            Email = "example@email.com"
        };
        _context.UserProfiles.Add(profile);
        await _context.SaveChangesAsync(cancellationToken);
        return profile;
    }

    public async Task UpdateProfileAsync(UserProfile profile, CancellationToken cancellationToken = default)
    {
        _context.UserProfiles.Update(profile);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
