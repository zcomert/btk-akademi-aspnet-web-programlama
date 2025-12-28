using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace News.Migrations
{
    /// <inheritdoc />
    public partial class StartPoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NewsArticles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(220)", maxLength: 220, nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsArticles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "NewsArticles",
                columns: new[] { "Id", "AuthorName", "Content", "PublishedAt", "Slug", "Status", "Summary", "Title" },
                values: new object[,]
                {
                    { 1, "BTK Akademi", "<p>Yapay zeka, medya kuruluşlarında içerik üretimini hızlandırmaya devam ediyor...</p>", new DateTime(2025, 1, 5, 9, 30, 0, 0, DateTimeKind.Utc), "yapay-zeka-destekli-haber-uretimi", "Published", "Yapay zeka destekli iş akışları dijital haber odalarını nasıl dönüştürüyor?", "Yapay Zeka Destekli Haber Üretimi" },
                    { 2, "Zafer Yazar", "<p>ASP.NET Core 9.0 güncellemesinde performans iyileştirmeleri ve API geliştirmeleri öne çıkıyor...</p>", new DateTime(2025, 2, 12, 14, 0, 0, 0, DateTimeKind.Utc), "aspnet-core-9-0-yenilikler", "Published", "ASP.NET Core 9.0 sürümündeki en önemli yenilikler.", "ASP.NET Core 9.0’daki Yenilikler" },
                    { 3, "Genel Yayın Yönetmeni", "<p>Bu hafta öne çıkan konulara odaklanıyor ve üretim planımızın detaylarını paylaşıyoruz...</p>", null, "editor-notlari", "Draft", "Haftalık editoryal öne çıkanlar ve yaklaşan içerik planları.", "Editör Notları" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewsArticles_Slug",
                table: "NewsArticles",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewsArticles");
        }
    }
}
