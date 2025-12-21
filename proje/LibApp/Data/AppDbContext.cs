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

    public override int SaveChanges()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BookAuthor>()
            .HasKey(x => new { x.BookId, x.AuthorId });

        modelBuilder.Entity<BookAuthor>()
            .HasOne(x => x.Book)
            .WithMany(x => x.BookAuthors)
            .HasForeignKey(x => x.BookId);

        modelBuilder.Entity<BookAuthor>()
            .HasOne(x => x.Author)
            .WithMany(x => x.BookAuthors)
            .HasForeignKey(x => x.AuthorId);

        modelBuilder.Entity<Book>()
            .HasIndex(x => x.ISBN)
            .IsUnique();

        SeedDefaults(modelBuilder);
    }

    private static void SeedDefaults(ModelBuilder modelBuilder)
    {
        var now = new DateTime(2024, 1, 1);

        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryId = 1, Name = "Software", Description = "Software engineering books" },
            new Category { CategoryId = 2, Name = "Science", Description = "Science and research" }
        );

        modelBuilder.Entity<Publisher>().HasData(
            new Publisher { PublisherId = 1, Name = "Tech Press", Website = "https://techpress.example.com" },
            new Publisher { PublisherId = 2, Name = "Science House", Website = "https://sciencehouse.example.com" }
        );

        modelBuilder.Entity<Author>().HasData(
            new Author { AuthorId = 1, FirstName = "Ada", LastName = "Lovelace", Biography = "Pioneer programmer" },
            new Author { AuthorId = 2, FirstName = "Nikola", LastName = "Tesla", Biography = "Inventor and engineer" }
        );

        modelBuilder.Entity<Book>().HasData(
            new Book
            {
                BookId = 1,
                Title = "Computing Basics",
                ISBN = "ISBN-001",
                CategoryId = 1,
                PublisherId = 1,
                PageCount = 250,
                PublishYear = 2020,
                Description = "A gentle introduction to computing.",
                CoverImageUrl = "https://example.com/covers/computing-basics.png",
                CreatedAt = now,
                UpdatedAt = now,
                IsActive = true
            },
            new Book
            {
                BookId = 2,
                Title = "Science Experiments",
                ISBN = "ISBN-002",
                CategoryId = 2,
                PublisherId = 2,
                PageCount = 320,
                PublishYear = 2021,
                Description = "Hands-on experiments.",
                CoverImageUrl = "https://example.com/covers/science-experiments.png",
                CreatedAt = now,
                UpdatedAt = now,
                IsActive = true
            }
        );

        modelBuilder.Entity<BookAuthor>().HasData(
            new BookAuthor { BookId = 1, AuthorId = 1 },
            new BookAuthor { BookId = 2, AuthorId = 2 }
        );
    }

    // Audit Trail
    // DRY
    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries<Book>()
            .Where(e => e.State is EntityState.Added or EntityState.Modified);

        foreach (var entry in entries)
        {
            entry.Entity.UpdatedAt = DateTime.UtcNow;
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }
        }
    }
}
