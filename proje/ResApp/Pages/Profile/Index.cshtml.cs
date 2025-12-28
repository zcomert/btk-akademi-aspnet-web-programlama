using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ResApp.Models;
using ResApp.Models.ViewModels;
using ResApp.Services;

namespace ResApp.Pages.Profile;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IUserProfileService _userProfileService;
    private readonly UserManager<User> _userManager;

    public IndexModel(IUserProfileService userProfileService, UserManager<User> userManager)
    {
        _userProfileService = userProfileService;
        _userManager = userManager;
    }

    [BindProperty]
    public ProfileInputModel Input { get; set; } = new();

    public string? FeedbackMessage { get; private set; }
    public bool IsSuccessMessage { get; private set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken)
    {
        await PopulateInputAsync(cancellationToken);
        FeedbackMessage = TempData["SuccessMessage"] as string ?? TempData["ErrorMessage"] as string;
        IsSuccessMessage = TempData.ContainsKey("SuccessMessage");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var userId = GetCurrentUserId();
        var profile = await _userProfileService.EnsureProfileAsync(userId, cancellationToken);
        profile.FullName = Input.FullName;
        profile.Email = Input.Email;
        profile.Phone = Input.Phone;
        profile.Address = Input.Address;
        profile.DateOfBirth = Input.DateOfBirth;

        await _userProfileService.UpdateProfileAsync(profile, cancellationToken);
        TempData["SuccessMessage"] = "Profil bilgileriniz güncellendi.";
        return RedirectToPage();
    }

    private async Task PopulateInputAsync(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var profile = await _userProfileService.EnsureProfileAsync(userId, cancellationToken);
        Input = new ProfileInputModel
        {
            FullName = profile.FullName,
            Email = profile.Email,
            Phone = profile.Phone,
            Address = profile.Address,
            DateOfBirth = profile.DateOfBirth
        };
    }

    private string GetCurrentUserId()
    {
        var idValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrWhiteSpace(idValue))
        {
            throw new InvalidOperationException("Kullanıcı bilgilerine erişilemedi.");
        }

        return idValue;
    }
}
