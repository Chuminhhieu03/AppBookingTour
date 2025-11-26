
namespace AppBookingTour.Application.Features.TourDepartures.CreateTourDeparture;

public class TourDepartureRequestDTO
{
    public DateTime? DepartureDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public int? AvailableSlots { get; set; }
    public decimal? PriceAdult { get; set; }
    public decimal? PriceChildren { get; set; }
    public int? Status { get; set; }
    public int? GuideId { get; set; }
}