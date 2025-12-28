using System.Text.Json.Serialization;

namespace ResApp.Models;

public class ApiBookDto
{
    [JsonPropertyName("bookId")]
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("authors")]
    public List<string>? Authors { get; set; }

    [JsonIgnore]
    public string Author => Authors is { Count: > 0 } ? string.Join(", ", Authors) : string.Empty;

    public string? Isbn { get; set; }

    public string? Description { get; set; }

    public string? CoverImageUrl { get; set; }
}
