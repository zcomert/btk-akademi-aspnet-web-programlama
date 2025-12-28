using System.ComponentModel.DataAnnotations;

namespace ResApp.Models.ViewModels;

public class ReservationFormViewModel
{
    public int BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Başlangıç Tarihi")]
    public DateTime StartDate { get; set; } = DateTime.Today;

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Bitiş Tarihi")]
    public DateTime EndDate { get; set; } = DateTime.Today.AddDays(7);

    public IReadOnlyList<Reservation> ExistingReservations { get; set; } = [];
}
