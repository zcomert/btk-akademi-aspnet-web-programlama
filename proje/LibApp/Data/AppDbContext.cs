using LibApp.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibApp.Data;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Publisher> Publishers => Set<Publisher>();
    public DbSet<BookAuthor> BookAuthors => Set<BookAuthor>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        :base(options)
    {
        
    }
}
