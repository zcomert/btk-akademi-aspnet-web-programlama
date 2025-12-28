using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using ResApp.Models;

namespace ResApp.Services;

public class BookApiClientService : IBookApiClientService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<BookApiClientService> _logger;

    private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public BookApiClientService(HttpClient httpClient, ILogger<BookApiClientService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<ApiBookDto>> GetAllBooksAsync(CancellationToken cancellationToken = default)
    {
        var books = await _httpClient.GetFromJsonAsync<List<ApiBookDto>>("api/books", _jsonSerializerOptions, cancellationToken);
        return books ?? [];
    }

    public async Task<ApiBookDto?> GetBookByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetFromJsonAsync<ApiBookDto>($"api/books/{id}", _jsonSerializerOptions, cancellationToken);
    }
}
