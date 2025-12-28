using System.ComponentModel.DataAnnotations;

namespace ResApp.Models;

public class Reservation
{
    public int Id { get; set; }

    public required string UserId { get; set; }

    public User? User { get; set; }

    [Required]
    public int BookId { get; set; }

    [Required, StringLength(256)]
    public string BookTitle { get; set; } = string.Empty;

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    public ReservationStatus Status { get; set; } = ReservationStatus.Active;
}
