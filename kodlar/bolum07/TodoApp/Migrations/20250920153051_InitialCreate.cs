using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TodoApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Todos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDone = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todos", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Todos",
                columns: new[] { "Id", "Description", "DueDate", "Priority", "Title" },
                values: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), "Pazartesi toplantısı için slaytlar", new DateTime(2025, 9, 23, 0, 0, 0, 0, DateTimeKind.Local), 2, "Sunum hazırla" });

            migrationBuilder.InsertData(
                table: "Todos",
                columns: new[] { "Id", "Description", "DueDate", "IsDone", "Title" },
                values: new object[] { new Guid("33333333-3333-3333-3333-333333333333"), "30 dk koşu", new DateTime(2025, 9, 22, 0, 0, 0, 0, DateTimeKind.Local), true, "Spor" });

            migrationBuilder.InsertData(
                table: "Todos",
                columns: new[] { "Id", "Description", "DueDate", "Priority", "Title" },
                values: new object[,]
                {
                    { new Guid("44444444-4444-4444-4444-444444444444"), "Yağ değişimi ve filtreler", new DateTime(2025, 9, 27, 0, 0, 0, 0, DateTimeKind.Local), 1, "Araba bakımı" },
                    { new Guid("e2799c83-6fb5-46d6-90b2-c6259f4400a8"), "Süt, ekmek, yumurta", new DateTime(2025, 9, 21, 0, 0, 0, 0, DateTimeKind.Local), 1, "Alışveriş yap" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Todos_DueDate",
                table: "Todos",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_Todos_IsDone",
                table: "Todos",
                column: "IsDone");

            migrationBuilder.CreateIndex(
                name: "IX_Todos_Priority",
                table: "Todos",
                column: "Priority");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Todos");
        }
    }
}
