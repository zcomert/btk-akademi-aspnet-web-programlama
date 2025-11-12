using ContactApp.Models;
using ContactApp.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContactApp.Repositories;
public class ApplicationDbContext 
    : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public DbSet<Contact> Contacts { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }
}
