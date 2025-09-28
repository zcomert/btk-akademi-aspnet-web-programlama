using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TodoApp.Repositories;
using TodoApp.Services;
using TodoApp.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<TodoValidator>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Server=(localdb)\\MSSQLLocalDB; Database=TodoAppDb; Trusted_Connection=True; TrustServerCertificate=True";
    

builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlServer(connectionString)
);

if (builder.Environment.IsDevelopment())
{
    // DI - Register
    builder
        .Services
        .AddSingleton<ITodoStore, InMemoryTodoStore>();
}
else
{
    builder
        .Services
        .AddScoped<ITodoStore, EfTodoStore>();
}


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if(app.Environment.IsProduction())
{
    using var scope = app.Services.CreateScope();
    try
    {
        var db = scope
            .ServiceProvider
            .GetRequiredService<TodoDbContext>();
        
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = scope
            .ServiceProvider
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("Startup");

        logger.LogError(ex, "Database migration failed.");
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//app.MapStaticAssets();
//app.MapRazorPages()
//   .WithStaticAssets();

app.MapRazorPages();

app.Run();
