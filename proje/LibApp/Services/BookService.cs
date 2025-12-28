using LibApp.Data;
using LibApp.Models.Entities;
using LibApp.Services.Intrefaces;
using Microsoft.EntityFrameworkCore;

namespace LibApp.Services;

// C# Primary Constructor Syntax
public class BookService(AppDbContext context, ILogger<BookService> logger) : IBookService
{
    private readonly AppDbContext _context = context;
    private readonly ILogger<BookService> _logger = logger;

    public async Task<List<Book>> GetAllAsync()
    {
        _logger.LogInformation("Retrieving all books.");
        return await _context.Books
            .Include(b => b.Category)
            .Include(b => b.Publisher)
            .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
            .OrderByDescending(b => b.UpdatedAt)
            .ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving book with ID: {BookId}", id);
        var book = await _context.Books
            .Include(b => b.Category)
            .Include(b => b.Publisher)
            .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
            .FirstOrDefaultAsync(b => b.BookId == id);

        if (book is null)
        {
            _logger.LogWarning("Book with ID: {BookId} not found.", id);
        }
        else
        {
            _logger.LogInformation("Book with ID: {BookId} retrieved successfully.", id);
        }
        return book;
    }

    public async Task<Book> CreateAsync(Book book, IEnumerable<int> authorIds)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            UpdateBookAuthors(book, authorIds);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            _logger.LogInformation("Book {Title} created successfully with ID: {BookId}", book.Title, book.BookId);
            return book;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error while creating book {Title}", book.Title);
            throw;
        }
    }

    public async Task UpdateAsync(Book book, IEnumerable<int> authorIds)
    {
        var existingBook = await _context.Books
            .Include(b => b.BookAuthors)
            .FirstOrDefaultAsync(b => b.BookId == book.BookId);

        if (existingBook is null)
        {
            _logger.LogWarning("Attempted to update book with ID: {BookId} but it was not found.", book.BookId);
            throw new InvalidOperationException("Book not found");
        }

        _logger.LogInformation("Updating book with ID: {BookId}", book.BookId);
        existingBook.Title = book.Title;
        existingBook.ISBN = book.ISBN;
        existingBook.CategoryId = book.CategoryId;
        existingBook.PublisherId = book.PublisherId;
        existingBook.Description = book.Description;
        existingBook.PublishYear = book.PublishYear;
        existingBook.PageCount = book.PageCount;
        existingBook.CoverImageUrl = book.CoverImageUrl;
        existingBook.IsActive = book.IsActive;

        UpdateBookAuthors(existingBook, authorIds);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Book with ID: {BookId} updated successfully.", book.BookId);
    }

    public async Task DeleteAsync(int id)
    {
        _logger.LogInformation("Attempting to delete book with ID: {BookId}", id);
        var book = await _context.Books.FindAsync(id);
        if (book is null)
        {
            _logger.LogWarning("Book with ID: {BookId} not found for deletion.", id);
            return;
        }

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Book with ID: {BookId} deleted successfully.", id);
    }

    public Task<List<Category>> GetCategoriesAsync() => _context.Categories.OrderBy(x => x.Name).ToListAsync();

    public Task<List<Publisher>> GetPublishersAsync() => _context.Publishers.OrderBy(x => x.Name).ToListAsync();

    public Task<List<Author>> GetAuthorsAsync() => _context.Authors.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToListAsync();

    private void UpdateBookAuthors(Book book, IEnumerable<int> authorIds)
    {
        var ids = authorIds.ToHashSet();
        book.BookAuthors.Clear();

        foreach (var authorId in ids)
        {
            book.BookAuthors.Add(new BookAuthor { BookId = book.BookId, AuthorId = authorId });
        }
    }
}