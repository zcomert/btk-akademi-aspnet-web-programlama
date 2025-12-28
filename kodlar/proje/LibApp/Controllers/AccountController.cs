using LibApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibApp.Controllers;

// C# Primary Constructor Syntax
public class AccountController(
    SignInManager<IdentityUser> signInManager,
    ILogger<AccountController> logger) : Controller
{
    private readonly SignInManager<IdentityUser> _signInManager = signInManager;
    private readonly ILogger<AccountController> _logger = logger;

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity is { IsAuthenticated: true })
        {
            return RedirectToAction("Index", "Books");
        }
        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var result = await _signInManager.PasswordSignInAsync(
            viewModel.Username,
            viewModel.Password,
            isPersistent: false,
            lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Geçersiz kullanıcı adı veya şifre.");
            _logger.LogWarning("Başarısız giriş denemesi: {Username}", viewModel.Username);
            return View(viewModel);
        }

        _logger.LogInformation("Kullanıcı {Username} giriş yaptı.", viewModel.Username);
        if (!string.IsNullOrEmpty(viewModel.ReturnUrl) && Url.IsLocalUrl(viewModel.ReturnUrl))
        {
            return Redirect(viewModel.ReturnUrl);
        }

        return RedirectToAction("Index", "Books");
    }


    // Logout action
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }
}
