using ContactApp.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace ContactApp.Services;

public class AuthManager : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthManager(UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<SignInResult> Login(string username, string password)
    {
        var result = await _signInManager
            .PasswordSignInAsync(username, password, false, false);
        return result;
    }

    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<IdentityResult> Register(ApplicationUser user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        return result;
    }
}
