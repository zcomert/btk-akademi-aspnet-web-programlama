using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ResApp.Data;
using ResApp.Models;
using ResApp.Options;
using ResApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<LibAppApiOptions>(builder.Configuration.GetSection(LibAppApiOptions.SectionName));
builder.Services.Configure<SeedUserOptions>(builder.Configuration.GetSection(SeedUserOptions.SectionName));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 8;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.Cookie.Name = "ResApp.Auth";
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();

builder.Services.AddHttpClient<IBookApiClientService, BookApiClientService>((sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<LibAppApiOptions>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(15);
});

var app = builder.Build();

await SeedData.EnsureSeededAsync(app.Services);

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();

app.Run();
