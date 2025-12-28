using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResApp.Models.ViewModels;
using ResApp.Services;

namespace ResApp.Controllers;

[Authorize]
public class ReservationsController : Controller
{
    private readonly IReservationService _reservationService;
    private readonly IBookApiClientService _bookApiClientService;

    public ReservationsController(IReservationService reservationService, IBookApiClientService bookApiClientService)
    {
        _reservationService = reservationService;
        _bookApiClientService = bookApiClientService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var reservations = await _reservationService.GetUserReservationsAsync(GetCurrentUserId(), cancellationToken);
        return View(reservations);
    }

    [HttpGet]
    public async Task<IActionResult> Create(int bookId, CancellationToken cancellationToken)
    {
        var book = await _bookApiClientService.GetBookByIdAsync(bookId, cancellationToken);
        if (book is null)
        {
            TempData["ErrorMessage"] = "Kitap bilgisi alınamadı.";
            return RedirectToAction("Index", "Books");
        }

        var existingReservations = await _reservationService.GetBookReservationsAsync(bookId, cancellationToken);

        return View(new ReservationFormViewModel
        {
            BookId = book.Id,
            BookTitle = book.Title,
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(7),
            ExistingReservations = existingReservations
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ReservationFormViewModel model, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var book = await _bookApiClientService.GetBookByIdAsync(model.BookId, cancellationToken);
        if (book is null)
        {
            TempData["ErrorMessage"] = "Kitap bilgisi alınamadı.";
            return RedirectToAction("Index", "Books");
        }

        var result = await _reservationService.CreateReservationAsync(GetCurrentUserId(), book, model.StartDate, model.EndDate, cancellationToken);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            model.BookTitle = book.Title;
            return View(model);
        }

        TempData["SuccessMessage"] = result.Message;
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id, CancellationToken cancellationToken)
    {
        var result = await _reservationService.CancelReservationAsync(id, GetCurrentUserId(), User.IsInRole("Admin"), cancellationToken);
        TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
        return User.IsInRole("Admin")
            ? RedirectToAction(nameof(Admin))
            : RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Admin(CancellationToken cancellationToken)
    {
        var reservations = await _reservationService.GetAllReservationsAsync(cancellationToken);
        return View(reservations);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> QuickReserve(int bookId, CancellationToken cancellationToken)
    {
        var book = await _bookApiClientService.GetBookByIdAsync(bookId, cancellationToken);
        if (book is null)
        {
            TempData["ErrorMessage"] = "Kitap bilgisi alınamadı.";
            return RedirectToAction("Index", "Books");
        }

        var startDate = DateTime.Today;
        var endDate = startDate.AddDays(2); // 3 gün
        var result = await _reservationService.CreateReservationAsync(GetCurrentUserId(), book, startDate, endDate, cancellationToken);
        TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;
        return RedirectToAction("Index", "Books");
    }

    private string GetCurrentUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            throw new InvalidOperationException("User ID not found in claims.");
        }
        return userId;
    }
}
