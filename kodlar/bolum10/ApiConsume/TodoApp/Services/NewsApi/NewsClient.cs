
namespace TodoApp.Services.NewsApi
{
    public class NewsClient : INewsClient
    {
        private readonly HttpClient _http;
        public NewsClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<NewsArticleDto?> GetArticleAsync(int id, 
            CancellationToken cancellationToken = default)
        {
            return await _http
                .GetFromJsonAsync<NewsArticleDto>($"api/NewsArticles/{id}", cancellationToken);
        }

        public async Task<IEnumerable<NewsArticleDto>> GetArticlesAsync(CancellationToken cancellationToken = default)
        {
            var result = await _http
                .GetFromJsonAsync<IEnumerable<NewsArticleDto>>("api/NewsArticles", cancellationToken);

            return result ?? Enumerable.Empty<NewsArticleDto>();
        }
    }
}
