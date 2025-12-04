namespace AppBookingTour.Application.Features.Tours.GetFeaturedTours;

public class FeaturedTourDTO
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int DurationDays { get; set; }
    public int DurationNights { get; set; }
    public decimal BasePriceAdult { get; set; }
    public decimal BasePriceChild { get; set; }
    public string DepartureCityName { get; set; } = "";
    public string DestinationCityName { get; set; } = "";
    public int TypeId { get; set; }
    public string TypeName { get; set; } = "";
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = "";
    public string? ImageMainUrl { get; set; }
    public decimal Rating { get; set; }
    public List<FeaturedTourDepartureItem> Departures { get; set; } = new List<FeaturedTourDepartureItem>();
}

public class FeaturedTourDepartureItem
{
    public int Id { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public int AvailableSlots { get; set; }
}
