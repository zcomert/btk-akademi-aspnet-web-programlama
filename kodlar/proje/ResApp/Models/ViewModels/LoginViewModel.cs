using System.ComponentModel.DataAnnotations;

namespace ResApp.Models.ViewModels;

public class LoginViewModel
{
    [Required, StringLength(64)]
    [Display(Name = "Kullanıcı Adı")]
    public string UserName { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    [Display(Name = "Şifre")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Beni Hatırla")]
    public bool RememberMe { get; set; }
}
