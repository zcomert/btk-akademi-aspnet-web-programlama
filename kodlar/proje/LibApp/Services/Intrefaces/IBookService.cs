using LibApp.Models.Entities;

namespace LibApp.Services.Intrefaces;

public interface IBookService
{
    Task<List<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int id);
    Task<Book> CreateAsync(Book book, IEnumerable<int> authorIds);
    Task UpdateAsync(Book book, IEnumerable<int> authorIds);
    Task DeleteAsync(int id);
    Task<List<Category>> GetCategoriesAsync();
    Task<List<Publisher>> GetPublishersAsync();
    Task<List<Author>> GetAuthorsAsync();
}
