using LibApp.Models.Entities;
using LibApp.Models.ViewModels;
using LibApp.Services.Intrefaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibApp.Controllers;

[Authorize(Roles = "Administrator")]
public class BooksController(IBookService bookService) : Controller
{
    private readonly IBookService _bookService = bookService;

    public async Task<IActionResult> Index()
    {
        var books = await _bookService.GetAllAsync();
        ViewBag.Username = User.Identity?.Name ?? "admin";
        return View(books);
    }

    public async Task<IActionResult> Create()
    {
        var viewModel = await BuildFormViewModelAsync(new BookFormViewModel());
        return View("Edit", viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Edit", await BuildFormViewModelAsync(model));
        }

        var book = MapToEntity(model);
        await _bookService.CreateAsync(book, model.SelectedAuthorIds);
        TempData["Message"] = "Kitap başarıyla oluşturuldu.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var book = await _bookService.GetByIdAsync(id);
        if (book is null)
        {
            return NotFound();
        }

        var model = new BookFormViewModel
        {
            BookId = book.BookId,
            Title = book.Title,
            ISBN = book.ISBN,
            CategoryId = book.CategoryId,
            PublisherId = book.PublisherId,
            Description = book.Description,
            PublishYear = book.PublishYear,
            PageCount = book.PageCount,
            CoverImageUrl = book.CoverImageUrl,
            IsActive = book.IsActive,
            SelectedAuthorIds = book.BookAuthors.Select(x => x.AuthorId).ToList()
        };

        return View(await BuildFormViewModelAsync(model));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, BookFormViewModel model)
    {
        if (id != model.BookId)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(await BuildFormViewModelAsync(model));
        }

        var book = MapToEntity(model);
        await _bookService.UpdateAsync(book, model.SelectedAuthorIds);
        TempData["Message"] = "Kitap başarıyla güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var book = await _bookService.GetByIdAsync(id);
        if (book is null)
        {
            return NotFound();
        }

        return View(book);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _bookService.DeleteAsync(id);
        TempData["Message"] = "Kitap başarıyla silindi.";
        return RedirectToAction(nameof(Index));
    }

    private async Task<BookFormViewModel> BuildFormViewModelAsync(BookFormViewModel model)
    {
        model.Categories = await _bookService.GetCategoriesAsync();
        model.Publishers = await _bookService.GetPublishersAsync();
        model.Authors = await _bookService.GetAuthorsAsync();
        if (!model.SelectedAuthorIds.Any() && model.Authors.Any())
        {
            model.SelectedAuthorIds.Add(model.Authors.First().AuthorId);
        }

        return model;
    }

    private static Book MapToEntity(BookFormViewModel model)
    {
        return new Book
        {
            BookId = model.BookId ?? 0,
            Title = model.Title,
            ISBN = model.ISBN,
            CategoryId = model.CategoryId,
            PublisherId = model.PublisherId,
            Description = model.Description,
            PublishYear = model.PublishYear,
            PageCount = model.PageCount,
            CoverImageUrl = model.CoverImageUrl,
            IsActive = model.IsActive
        };
    }
}
