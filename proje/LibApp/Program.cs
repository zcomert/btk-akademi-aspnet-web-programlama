using LibApp.Data;
using LibApp.Models.Options;
using LibApp.Services;
using LibApp.Services.Intrefaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Options Pattern
builder.Services.Configure<AdminUserOptions>(builder.Configuration.GetSection("AdminUser"));

// Veri tabanı için gerekli olan hizmetleri eklenmesi
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// BookService kaydı
builder.Services.AddScoped<IBookService, BookService>();

// Identity kaydı
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Cookie ayarları
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Login";
    options.Cookie.Name = "LibApp";
});

builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddControllersWithViews();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
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
app.UseCors("AllowAll");

using (var scope = app.Services.CreateScope())
{
    var context = scope
        .ServiceProvider
        .GetRequiredService<AppDbContext>();

    context.Database.Migrate();

    var adminOptions = scope
        .ServiceProvider
        .GetRequiredService<IOptions<AdminUserOptions>>().Value;

    AdminUserSeeder.SeedAdminUserAsync(context, adminOptions)
        .GetAwaiter()
        .GetResult();
}

// oturum açma ve yetkilendirme ara katman yazılımlarının etkinleştirilmesi
app.UseAuthentication();
app.UseAuthorization();


app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllers();

app.Run();
