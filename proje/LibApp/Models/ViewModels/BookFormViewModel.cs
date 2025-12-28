using LibApp.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace LibApp.Models.ViewModels;

public class BookFormViewModel
{
    public int? BookId { get; set; }

    [Required, StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(20)]
    public string ISBN { get; set; } = string.Empty;

    [Url]
    public string? CoverImageUrl { get; set; }

    [Range(1500, 3000)]
    public int? PublishYear { get; set; }

    [Range(1, 5000)]
    public int? PageCount { get; set; }

    [StringLength(2000)]
    public string? Description { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public int PublisherId { get; set; }

    [Required]
    public List<int> SelectedAuthorIds { get; set; } = new();

    public bool IsActive { get; set; } = true;

    public IEnumerable<Category> Categories { get; set; } = Enumerable.Empty<Category>();

    public IEnumerable<Publisher> Publishers { get; set; } = Enumerable.Empty<Publisher>();

    public IEnumerable<Author> Authors { get; set; } = Enumerable.Empty<Author>();
}
