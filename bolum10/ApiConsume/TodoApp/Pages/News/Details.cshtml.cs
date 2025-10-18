using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TodoApp.Services.NewsApi;

namespace TodoApp.Pages.News
{
    public class DetailsModel : PageModel
    {
        private readonly INewsClient _newsClient;

        public NewsArticleDto? Article { get; private set; }
        public DetailsModel(INewsClient newsClient)
        {
            _newsClient = newsClient;
        }

        public async Task<IActionResult> OnGet(int id, CancellationToken cancellationToken)
        {
            Article = await _newsClient
                .GetArticleAsync(id, cancellationToken);
            if (Article is null)
                return NotFound(); // 404
            return Page();
        }
    }
}
