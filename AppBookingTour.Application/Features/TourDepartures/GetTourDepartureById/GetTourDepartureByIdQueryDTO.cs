
namespace AppBookingTour.Application.Features.TourDepartures.GetTourDepartureById;

public class TourDepartureDTO
{
    public int Id { get; set; }
    public int TourId { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public decimal PriceAdult { get; set; }
    public decimal PriceChildren { get; set; }
    public int AvailableSlots { get; set; }
    public string? GuideName { get; set; }
    public int Status { get; set; }
}