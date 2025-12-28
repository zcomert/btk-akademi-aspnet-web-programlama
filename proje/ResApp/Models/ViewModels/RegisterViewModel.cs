using System.ComponentModel.DataAnnotations;

namespace ResApp.Models.ViewModels;

public class RegisterViewModel
{
    [Required, StringLength(64)]
    [Display(Name = "Kullanıcı Adı")]
    public string UserName { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    [Display(Name = "Şifre")]
    public string Password { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Şifreler uyuşmuyor.")]
    [Display(Name = "Şifre Tekrarı")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Required, StringLength(128)]
    [Display(Name = "Ad Soyad")]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    [Display(Name = "E-posta")]
    public string Email { get; set; } = string.Empty;
}
