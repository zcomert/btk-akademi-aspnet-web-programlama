namespace ContactApp.Contracts.DTOs;

public class NewsArticleDto
{
    public int Id { get; set; }
    public String Title { get; set; } = String.Empty;
    public String Slug { get; set; } = String.Empty;
    public String? Summary { get; set; }
    public String Content { get; set; } = String.Empty;
    public DateTime? PublishedAt { get; set; }
    public int Status { get; set; }
    public String AuthorName { get; set; } = String.Empty;

}
