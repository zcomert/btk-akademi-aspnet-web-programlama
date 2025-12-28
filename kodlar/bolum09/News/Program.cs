using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using News.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("NewsDb")
        ?? "Server=(localdb)\\MSSQLLocalDB;Database=NewsDb;Trusted_Connection=True;TrustServerCertificate=True";
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    options.SwaggerDoc("v1", new OpenApiInfo()
    {
       Title = "Haberler API",
       Version = "v1",
       Description = "BTK Akademi Haberler için Swagger belgesi"
    })
);
builder.Services.AddOpenApi();

builder.Services.AddDbContext<NewsDbContext>(options =>
    options.UseSqlServer(connectionString)
);

builder.Services.AddScoped<INewsRepository, NewsRepository>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContex = scope.ServiceProvider.GetRequiredService<NewsDbContext>();
    dbContex.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json","Haberler API v1");
    });
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
