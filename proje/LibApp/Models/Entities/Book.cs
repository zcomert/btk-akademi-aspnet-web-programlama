using System.ComponentModel.DataAnnotations;

namespace LibApp.Models.Entities;

public class Book
{
    public int BookId { get; set; }

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

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public int PublisherId { get; set; }

    public Category? Category { get; set; }

    public Publisher? Publisher { get; set; }

    public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
}

