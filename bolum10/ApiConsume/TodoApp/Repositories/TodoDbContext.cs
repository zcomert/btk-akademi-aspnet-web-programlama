using Microsoft.EntityFrameworkCore;
using TodoApp.Models;
using TodoApp.Repositories.Config;

namespace TodoApp.Repositories;

public class TodoDbContext(DbContextOptions<TodoDbContext> options) 
    : DbContext(options)
{
    public DbSet<Todo> Todos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.ApplyConfiguration(new TodoConfiguration());
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TodoDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

}
