using System.ComponentModel.DataAnnotations;

namespace ContactApp.Models.ViewModels;

public class RoleViewModel
{
    public string? Id { get; set; }
    
    [Required]
    public string? Name { get; set; }
}
