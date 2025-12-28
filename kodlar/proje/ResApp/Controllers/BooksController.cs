using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResApp.Services;

namespace ResApp.Controllers;

[Authorize]
public class BooksController : Controller
{
    private readonly IBookApiClientService _bookApiClientService;

    public BooksController(IBookApiClientService bookApiClientService)
    {
        _bookApiClientService = bookApiClientService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var books = await _bookApiClientService.GetAllBooksAsync(cancellationToken);
        return View(books);
    }

    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var book = await _bookApiClientService.GetBookByIdAsync(id, cancellationToken);
        if (book is null)
        {
            TempData["ErrorMessage"] = "Kitap bilgisi bulunamadı.";
            return RedirectToAction(nameof(Index));
        }

        return View(book);
    }
}
