using Microsoft.AspNetCore.Identity;

namespace ResApp.Models;

public class User : IdentityUser
{
    public UserProfile? Profile { get; set; }
}
