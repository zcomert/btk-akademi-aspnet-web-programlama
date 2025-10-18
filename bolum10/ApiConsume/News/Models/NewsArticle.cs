using System.ComponentModel.DataAnnotations;

namespace News.Models;

public class NewsArticle
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [StringLength(220)]
    public string Slug { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string Summary { get; set; } = string.Empty;
    
    [Required]
    public string Content { get; set; } = string.Empty;
    public DateTime? PublishedAt { get; set; }

    [Required]
    public NewsStatus Status { get; set; } = NewsStatus.Draft;
    
    [Required]
    [StringLength(200)]
    public string AuthorName { get; set; } = string.Empty;

}
public enum NewsStatus
{
    Draft,
    Published,
    Archived
}

