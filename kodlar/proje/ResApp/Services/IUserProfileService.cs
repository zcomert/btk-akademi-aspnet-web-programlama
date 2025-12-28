using ResApp.Models;

namespace ResApp.Services;

public interface IUserProfileService
{
    Task<UserProfile?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<UserProfile> EnsureProfileAsync(string userId, CancellationToken cancellationToken = default);
    Task UpdateProfileAsync(UserProfile profile, CancellationToken cancellationToken = default);
}
