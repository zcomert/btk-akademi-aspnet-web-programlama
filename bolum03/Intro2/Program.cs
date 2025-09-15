using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Hata Yönetimi
app.UseExceptionHandler(exceptionApp =>
{
    exceptionApp.Run(async context =>
    {
        var feature = context.Features.Get<IExceptionHandlerPathFeature>();
        var ex = feature?.Error;

        context.Response.StatusCode = 500;
        context.Response.ContentType = "text/plain; charset=utf-8";

        await context.Response.WriteAsync("Sunucu hatası oluştu!\n");
        System.Console.WriteLine($"[HATA]  İstek Yolu{feature?.Path} | Mesaj: {ex?.Message}");
    });
});

// Süre ölçme
app.Use(async (context, next) =>
{
    var sw = Stopwatch.StartNew();

    context.Response.OnStarting(() =>
    {
        sw.Stop();
        context.Response.Headers["X-Elapsed-Milisecond"] = sw.ElapsedMilliseconds.ToString();
        return Task.CompletedTask;
    });

    await next.Invoke(); // zincire devam edecek.
    System.Console.WriteLine($"[SÜRE] {context.Request.Method} {context.Request.Path} --> {sw.ElapsedMilliseconds} ms");
});

app.MapGet("/", () => "Hello World!");


app.MapGet("/hello", () =>
{
    return Results.Text("Merhaba! Bu bir middleware odaklı .NET 9.0 deneme uygulamasıdır.");
});

app.MapGet("/boom", () =>
{
    throw new InvalidOperationException("Eğitim amaçlı kontrollü hata!");
});

app.Run();
