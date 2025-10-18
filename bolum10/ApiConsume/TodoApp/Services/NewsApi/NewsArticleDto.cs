namespace TodoApp.Services.NewsApi
{
    public class NewsArticleDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime? PublishedAt { get; set; }
        public string AuthorName { get; set; } = string.Empty;
    }
}
