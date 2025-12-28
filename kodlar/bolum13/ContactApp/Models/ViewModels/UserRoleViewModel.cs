using ContactApp.Models.ViewModels;

namespace ContactApp.Models.ViewModels;
public class UserRoleViewModel
{
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public List<RoleViewModel>? Roles { get; set; }
    public List<string>? UserRoles { get; set; }
}

