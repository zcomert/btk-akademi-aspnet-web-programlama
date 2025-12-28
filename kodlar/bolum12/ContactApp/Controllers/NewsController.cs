using ContactApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactApp.Controllers
{
    [Route("news")]
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var articles = await _newsService.GetAllAsync(cancellationToken);
            return View(articles);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {
            var article = await _newsService.GetByIdAsync(id, cancellationToken);
            if(article is null)
            {
                return NotFound(); // 404
            }
            return View(article);
        }
    }
}
