using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ResApp.Data;
using ResApp.Models;

namespace ResApp.Services;

public class ReservationService : IReservationService
{
    private const int ReservationDayLimit = 7;
    private readonly AppDbContext _context;
    private readonly ILogger<ReservationService> _logger;

    public ReservationService(AppDbContext context, ILogger<ReservationService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IReadOnlyList<Reservation>> GetUserReservationsAsync(string userId, CancellationToken cancellationToken = default)
    {
        await MarkExpiredReservationsAsync(cancellationToken);

        return await _context.Reservations
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.StartDate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Reservation>> GetAllReservationsAsync(CancellationToken cancellationToken = default)
    {
        await MarkExpiredReservationsAsync(cancellationToken);

        return await _context.Reservations
            .Include(r => r.User)
            .OrderByDescending(r => r.StartDate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Reservation>> GetBookReservationsAsync(int bookId, CancellationToken cancellationToken = default)
    {
        return await _context.Reservations
            .Where(r => r.BookId == bookId && r.Status == ReservationStatus.Active)
            .OrderBy(r => r.StartDate)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<ServiceResult> CreateReservationAsync(string userId, ApiBookDto book, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        startDate = startDate.Date;
        endDate = endDate.Date;

        if (endDate < startDate)
        {
            return ServiceResult.Failed("Bitiş tarihi başlangıç tarihinden önce olamaz.");
        }

        if ((endDate - startDate).TotalDays >= ReservationDayLimit + 1)
        {
            return ServiceResult.Failed($"Rezervasyon süresi en fazla {ReservationDayLimit} gün olabilir.");
        }

        var now = DateTime.UtcNow.Date;
        if (startDate < now)
        {
            return ServiceResult.Failed("Geçmiş bir tarih için rezervasyon yapılamaz.");
        }

        bool alreadyReserved = await _context.Reservations
            .AnyAsync(r =>
                    r.BookId == book.Id &&
                    r.Status == ReservationStatus.Active &&
                    r.StartDate <= endDate &&
                    r.EndDate >= startDate,
                cancellationToken);

        if (alreadyReserved)
        {
            return ServiceResult.Failed("Belirtilen tarih aralığında kitap için aktif bir rezervasyon mevcut.");
        }

        var reservation = new Reservation
        {
            UserId = userId,
            BookId = book.Id,
            BookTitle = book.Title,
            StartDate = startDate,
            EndDate = endDate,
            Status = ReservationStatus.Active
        };

        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Yeni rezervasyon oluşturuldu. Kullanıcı: {UserId}, Kitap: {BookId}", userId, book.Id);

        return ServiceResult.Successful("Rezervasyon talebiniz alındı.");
    }

    public async Task<ServiceResult> CancelReservationAsync(int reservationId, string requesterUserId, bool isAdmin, CancellationToken cancellationToken = default)
    {
        var reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == reservationId, cancellationToken);

        if (reservation is null)
        {
            return ServiceResult.Failed("Rezervasyon bulunamadı.");
        }

        if (!isAdmin && reservation.UserId != requesterUserId)
        {
            return ServiceResult.Failed("Bu rezervasyonu iptal etme yetkiniz yok.");
        }

        if (reservation.Status != ReservationStatus.Active)
        {
            return ServiceResult.Failed("Sadece aktif rezervasyonlar iptal edilebilir.");
        }

        reservation.Status = ReservationStatus.Cancelled;
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Rezervasyon iptal edildi. Rezervasyon Id: {ReservationId}", reservationId);
        return ServiceResult.Successful("Rezervasyon iptal edildi.");
    }

    private async Task MarkExpiredReservationsAsync(CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow.Date;
        var expiredReservations = await _context.Reservations
            .Where(r => r.Status == ReservationStatus.Active && r.EndDate < now)
            .ToListAsync(cancellationToken);

        if (expiredReservations.Count == 0)
        {
            return;
        }

        foreach (var reservation in expiredReservations)
        {
            reservation.Status = ReservationStatus.Expired;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
