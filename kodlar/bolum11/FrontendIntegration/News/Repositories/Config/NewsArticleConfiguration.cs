using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using News.Models;

namespace News.Repositories.Config
{
    public class NewsArticleConfiguration : IEntityTypeConfiguration<NewsArticle>
    {
        public void Configure(EntityTypeBuilder<NewsArticle> builder)
        {
            builder.HasKey(article => article.Id);

            builder.Property(article => article.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(article => article.Slug)
               .IsRequired()
               .HasMaxLength(220);

            builder.HasIndex(article => article.Slug)
                .IsUnique();

            builder.Property(article => article.Summary)
              .HasMaxLength(500);

            builder.Property(article => article.Content)
              .IsRequired();

            builder.Property(article => article.AuthorName)
              .IsRequired()
              .HasMaxLength(100);

            builder.Property(article => article.Status)
                .HasConversion<string>()
                .IsRequired();

            builder.HasData(
                new NewsArticle
                {
                    Id = 1,
                    Title = "Yapay Zeka Destekli Haber Üretimi",
                    Slug = "yapay-zeka-destekli-haber-uretimi",
                    Summary = "Yapay zeka destekli iş akışları dijital haber odalarını nasıl dönüştürüyor?",
                    Content = "<p>Yapay zeka, medya kuruluşlarında içerik üretimini hızlandırmaya devam ediyor...</p>",
                    PublishedAt = new DateTime(2025, 1, 5, 9, 30, 0, DateTimeKind.Utc),
                    Status = NewsStatus.Published,
                    AuthorName = "BTK Akademi"
                },
                new NewsArticle
                {
                    Id = 2,
                    Title = "ASP.NET Core 9.0’daki Yenilikler",
                    Slug = "aspnet-core-9-0-yenilikler",
                    Summary = "ASP.NET Core 9.0 sürümündeki en önemli yenilikler.",
                    Content = "<p>ASP.NET Core 9.0 güncellemesinde performans iyileştirmeleri ve API geliştirmeleri öne çıkıyor...</p>",
                    PublishedAt = new DateTime(2025, 2, 12, 14, 0, 0, DateTimeKind.Utc),
                    Status = NewsStatus.Published,
                    AuthorName = "Zafer Yazar"
                },
                new NewsArticle
                {
                    Id = 3,
                    Title = "Editör Notları",
                    Slug = "editor-notlari",
                    Summary = "Haftalık editoryal öne çıkanlar ve yaklaşan içerik planları.",
                    Content = "<p>Bu hafta öne çıkan konulara odaklanıyor ve üretim planımızın detaylarını paylaşıyoruz...</p>",
                    PublishedAt = null,
                    Status = NewsStatus.Draft,
                    AuthorName = "Genel Yayın Yönetmeni"
                }
            );
        }
    }
}
