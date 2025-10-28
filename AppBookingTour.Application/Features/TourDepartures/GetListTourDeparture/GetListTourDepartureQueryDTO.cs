
namespace AppBookingTour.Application.Features.TourDepartures.GetListTourDeparture;

public class ListTourDepartureItem
{
    public int Id { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public decimal PriceAdult { get; set; }
    public int AvailableSlots { get; set; }
    public string Status { get; set; } = null!;
}
