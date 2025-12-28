using System.ComponentModel.DataAnnotations;

namespace LibApp.Models.Entities;

public class Category
{
    public int CategoryId { get; set; }

    [Required, StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public ICollection<Book> Books { get; set; } = new List<Book>();
}

