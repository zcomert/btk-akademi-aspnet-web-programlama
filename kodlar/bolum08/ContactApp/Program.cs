using ContactApp.Repositories;
using ContactApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DI - Register

builder.Services.AddDbContext<ContactDbContext>(options =>
{
    var connStr = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=App_Data/contacts.db";
    options.UseSqlite(connStr);
});

//builder
//    .Services
//    .AddSingleton<IContactRepository, InMemoryContactRepository>();

builder
    .Services
    .AddScoped<IContactRepository, EfContactRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


using (var scope = app.Services.CreateScope())
{
    var dataDir = Path.Combine(app.Environment.ContentRootPath, "App_Data");
    Directory.CreateDirectory(dataDir);

    var db = scope.ServiceProvider.GetRequiredService<ContactDbContext>();
    db.Database.Migrate();
    DbSeeder.Seed(db);
}

app.Run();
