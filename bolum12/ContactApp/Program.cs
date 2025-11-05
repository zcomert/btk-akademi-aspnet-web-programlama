using ContactApp.Models.Identity;
using ContactApp.Repositories;
using ContactApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// DI - Register

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connStr = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? "Data Source=App_Data/contacts.db";
    options.UseSqlite(connStr);
});

//builder
//    .Services
//    .AddSingleton<IContactRepository, InMemoryContactRepository>();

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;

    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder
    .Services
    .AddScoped<IContactRepository, EfContactRepository>();

builder
    .Services
    .AddScoped<IAuthService, AuthManager>();

var apiBaseUrl = builder.Configuration["ApiBaseUrl"]
    ?? "https://localhost:7208/";

builder.Services.AddHttpClient<INewsService, NewsService>(client =>
{
   client.BaseAddress = new Uri(apiBaseUrl);
});

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

app.UseAuthentication();
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

    var db = scope
            .ServiceProvider
            .GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
    
    // Kullanıcı işlemleri
    var userManager = scope
        .ServiceProvider
        .GetRequiredService<UserManager<ApplicationUser>>();

    var roleManager = scope
        .ServiceProvider
        .GetRequiredService<RoleManager<ApplicationRole>>();

    DbSeeder.Seed(db, userManager, roleManager);
}

app.Run();
