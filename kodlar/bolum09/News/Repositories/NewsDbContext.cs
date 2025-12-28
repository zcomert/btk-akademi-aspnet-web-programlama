using Microsoft.EntityFrameworkCore;
using News.Models;
using News.Repositories.Config;
using System.Diagnostics;

namespace News.Repositories;

public class NewsDbContext : DbContext
{
    public NewsDbContext(DbContextOptions<NewsDbContext> options)
        : base(options)
    {
        
    }

    public DbSet<NewsArticle> NewsArticles => Set<NewsArticle>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new NewsArticleConfiguration());
    }
}
