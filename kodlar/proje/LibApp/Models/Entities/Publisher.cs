using System.ComponentModel.DataAnnotations;

namespace LibApp.Models.Entities;

public class Publisher
{
    public int PublisherId { get; set; }

    [Required, StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [Url]
    public string? Website { get; set; }

    public ICollection<Book> Books { get; set; } = new List<Book>();
}

