using ResApp.Models;

namespace ResApp.Services;

public interface IBookApiClientService
{
    Task<List<ApiBookDto>> GetAllBooksAsync(CancellationToken cancellationToken = default);
    Task<ApiBookDto?> GetBookByIdAsync(int id, CancellationToken cancellationToken = default);
}
