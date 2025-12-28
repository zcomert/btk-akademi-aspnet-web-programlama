using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.Models;

namespace TodoApp.Repositories.Config
{
    public class TodoConfiguration : IEntityTypeConfiguration<Todo>
    {
        public void Configure(EntityTypeBuilder<Todo> builder)
        {
            builder.ToTable("Todos");
            
            builder.HasKey(x => x.Id); // Primary key

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.Property(x => x.Priority)
                .HasConversion<int>()
                .HasDefaultValue(TodoPriority.Low)
                .IsRequired();

            builder.Property(x => x.IsDone)
                .HasDefaultValue (false)
                .IsRequired();

            builder.HasIndex(x => x.DueDate);
            builder.HasIndex(x => x.Priority);
            builder.HasIndex(x => x.IsDone);

            var today = DateTime.Today;
            builder.HasData(
                new Todo
                {
                    Id = Guid.Parse("e2799c83-6fb5-46d6-90b2-c6259f4400a8"),
                    Title = "Alışveriş yap",
                    Description = "Süt, ekmek, yumurta",
                    Priority = TodoPriority.Medium,
                    DueDate = today.AddDays(1),
                    IsDone = false
                },
            new Todo
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Title = "Sunum hazırla",
                Description = "Pazartesi toplantısı için slaytlar",
                Priority = TodoPriority.High,
                DueDate = today.AddDays(3),
                IsDone = false
            },
            new Todo
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Title = "Spor",
                Description = "30 dk koşu",
                Priority = TodoPriority.Low,
                DueDate = today.AddDays(2),
                IsDone = true
            },
            new Todo
            {
                Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                Title = "Araba bakımı",
                Description = "Yağ değişimi ve filtreler",
                Priority = TodoPriority.Medium,
                DueDate = today.AddDays(7),
                IsDone = false
            }
            );
        }
    }
}
