using System.ComponentModel.DataAnnotations;

namespace LibApp.Models.Entities;
public class Author
{
    public int AuthorId { get; set; }

    [Required, StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    public DateTime? DateOfBirth { get; set; }

    [StringLength(2000)]
    public string? Biography { get; set; }

    // Collection navigation property
    public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
}

