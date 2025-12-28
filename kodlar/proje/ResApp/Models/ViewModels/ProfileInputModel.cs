using System.ComponentModel.DataAnnotations;

namespace ResApp.Models.ViewModels;

public class ProfileInputModel
{
    [Required, StringLength(128)]
    [Display(Name = "Ad Soyad")]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    [Display(Name = "E-posta")]
    public string Email { get; set; } = string.Empty;

    [Phone]
    [Display(Name = "Telefon")]
    public string? Phone { get; set; }

    [StringLength(256)]
    [Display(Name = "Adres")]
    public string? Address { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Doğum Tarihi")]
    public DateTime? DateOfBirth { get; set; }
}
