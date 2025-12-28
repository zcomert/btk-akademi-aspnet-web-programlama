using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoApp.Services.NewsApi;

namespace TodoApp.Pages.News
{
    public class IndexModel : PageModel
    {
        private readonly INewsClient _newsClient;

        public IndexModel(INewsClient newsClient)
        {
            _newsClient = newsClient;
        }

        public IEnumerable<NewsArticleDto> Articles { get; set; } = Enumerable.Empty<NewsArticleDto>();
        public async Task OnGet(CancellationToken cancellationToken)
        {
            Articles = await _newsClient.GetArticlesAsync(cancellationToken);
        }
    }
}
