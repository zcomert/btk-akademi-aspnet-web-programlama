using ResApp.Models;

namespace ResApp.Services;

public interface IReservationService
{
    Task<IReadOnlyList<Reservation>> GetBookReservationsAsync(int bookId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Reservation>> GetUserReservationsAsync(string userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Reservation>> GetAllReservationsAsync(CancellationToken cancellationToken = default);
    Task<ServiceResult> CreateReservationAsync(string userId, ApiBookDto book, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<ServiceResult> CancelReservationAsync(int reservationId, string requesterUserId, bool isAdmin, CancellationToken cancellationToken = default);
}
