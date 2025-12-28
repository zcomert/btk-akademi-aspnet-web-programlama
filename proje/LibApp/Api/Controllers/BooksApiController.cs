using LibApp.Models.Entities;
using LibApp.Services.Intrefaces;
using Microsoft.AspNetCore.Mvc;

namespace LibApp.Api.Controllers;

[ApiController]
[Route("api/books")]
public class BooksApiController(IBookService bookService, 
    ILogger<BooksApiController> logger) : ControllerBase
{
    private readonly IBookService _bookService = bookService;
    private readonly ILogger<BooksApiController> _logger = logger;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
    {
        var books = await _bookService.GetAllAsync();
        _logger.LogInformation("Books exported via API at {Timestamp}", DateTimeOffset.UtcNow);
        return Ok(books.Select(ProjectBook));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<object>> GetBook(int id)
    {
        var book = await _bookService.GetByIdAsync(id);
        if (book is null)
        {
            _logger.LogWarning("Book with id {BookId} not found", id);
            return NotFound();
        }

        _logger.LogInformation("Book {BookId} fetched via API at {Timestamp}", id, DateTimeOffset.UtcNow);
        return Ok(ProjectBook(book));
    }

    private static object ProjectBook(Book book) => new
    {
        book.BookId,
        book.Title,
        book.ISBN,
        book.PublishYear,
        book.PageCount,
        book.Description,
        Category = book.Category?.Name,
        Publisher = book.Publisher?.Name,
        Authors = book.BookAuthors.Select(x => $"{x.Author?.FirstName} {x.Author?.LastName}"),
        book.IsActive,
        book.CoverImageUrl
    };
}
