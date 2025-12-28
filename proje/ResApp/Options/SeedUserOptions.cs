namespace ResApp.Options;

public class SeedUserOptions
{
    public const string SectionName = "SeedUser";

    public required List<UserCredentials> Users { get; set; } = [];

    public class UserCredentials
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
        public required string FullName { get; set; }
    }
}
