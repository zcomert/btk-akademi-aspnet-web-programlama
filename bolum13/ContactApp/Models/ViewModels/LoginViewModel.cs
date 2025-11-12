using System.ComponentModel.DataAnnotations;

namespace ContactApp.Models.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Kullanıcı adı boş bırakılamaz.")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Şifre boş bırakılamaz.")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public string? ReturnUrl { get; set; }
}

