using System.ComponentModel.DataAnnotations;

namespace ResApp.Models;

public class UserProfile
{
    public int Id { get; set; }

    [Required]
    public required string UserId { get; set; }

    public User? User { get; set; }

    [Required, StringLength(128)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(128)]
    public string Email { get; set; } = string.Empty;

    [Phone, StringLength(32)]
    public string? Phone { get; set; }

    [StringLength(256)]
    public string? Address { get; set; }

    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }
}
