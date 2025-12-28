using Microsoft.EntityFrameworkCore;
using News.Models;

namespace News.Repositories;

public class NewsRepository : INewsRepository
{
    private readonly NewsDbContext _context;

    public NewsRepository(NewsDbContext context)
    {
        _context = context;
    }

    public async Task<NewsArticle> AddAsync(NewsArticle article, CancellationToken cancellationToken = default)
    {
        await _context.NewsArticles.AddAsync(article, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return article;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _context
            .NewsArticles
            .FindAsync(new object[] {id}, cancellationToken);

        if (entity is null)
            return;

        _context.NewsArticles.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<NewsArticle>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.NewsArticles
            .AsNoTracking()
            .OrderByDescending(x => x.PublishedAt)
            .ThenBy(article => article.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<NewsArticle?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.NewsArticles
            .AsNoTracking()
            .FirstOrDefaultAsync(article => article.Id.Equals(id), cancellationToken);
    }

    public async Task UpdateAsync(NewsArticle article, CancellationToken cancellationToken = default)
    {
        _context.NewsArticles.Update(article);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
