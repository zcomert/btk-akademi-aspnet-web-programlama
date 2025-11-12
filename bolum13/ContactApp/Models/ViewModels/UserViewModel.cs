using System.ComponentModel.DataAnnotations;

namespace ContactApp.Models.ViewModels;
public class UserViewModel
{
    public string? Id { get; set; }

    [Required]
    public string? UserName { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [DataType(DataType.Password)]
    public string? Password { get; set; }
}

