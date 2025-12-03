namespace AppBookingTour.Application.Features.TourDepartures.GetListTourDepartureForGuide;

public class TourDepartureItemForGuide
{
    public int Id { get; set; }
    public int TourId { get; set; }
    public string TourCode { get; set; } = null!;
    public string TourName { get; set; } = null!;
    public DateTime DepartureDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public string DepartureCityName { get; set; } = null!;
    public string DestinationCityName { get; set; } = null!;
    public int BookedSlots { get; set; }
}
