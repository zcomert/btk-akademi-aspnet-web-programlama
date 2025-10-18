using News.Models;

namespace News.Repositories;
public interface INewsRepository
{
    Task<List<NewsArticle>> GetAllAsync(CancellationToken cancellationToken=default);
    Task<NewsArticle?> GetByIdAsync(int id, CancellationToken cancellationToken=default);
    Task<NewsArticle> AddAsync(NewsArticle article, CancellationToken cancellationToken=default);
    Task UpdateAsync(NewsArticle article, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
