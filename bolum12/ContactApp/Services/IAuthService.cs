using ContactApp.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace ContactApp.Services;

public interface IAuthService
{
    Task<SignInResult> Login(string username, string password);
    Task Logout();
    Task<IdentityResult> Register(ApplicationUser user, string password);
}
