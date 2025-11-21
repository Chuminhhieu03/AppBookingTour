namespace AppBookingTour.Application.Features.Tours.GetFeaturedTours;

public class FeaturedTourDTO
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int DurationDays { get; set; }
    public int DurationNights { get; set; }
    public decimal BasePriceAdult { get; set; }
    public decimal? Rating { get; set; }
    public string? ImageMainUrl { get; set; }
    public string DepartureCityName { get; set; } = null!;
    public string DestinationCityName { get; set; } = null!;
    public string TypeName { get; set; } = null!;
}
