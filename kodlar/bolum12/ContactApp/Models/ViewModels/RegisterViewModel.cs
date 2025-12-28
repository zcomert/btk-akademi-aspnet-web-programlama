using System.ComponentModel.DataAnnotations;

namespace ContactApp.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adı boş bırakılamaz.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email boş bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre boş bırakılamaz.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor.")]
        public string ConfirmPassword { get; set; }
    }
}
