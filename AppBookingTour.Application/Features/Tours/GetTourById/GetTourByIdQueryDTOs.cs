using AppBookingTour.Application.Features.TourDepartures.GetTourDepartureById;
using AppBookingTour.Application.Features.TourItineraries.GetTourItineraryById;

namespace AppBookingTour.Application.Features.Tours.GetTourById;

public class TourDTO
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int TypeId { get; set; }
    public string TypeName { get; set; } = null!;
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public int DepartureCityId { get; set; }
    public string DepartureCityName { get; set; } = null!;
    public int DestinationCityId { get; set; }
    public string DestinationCityName { get; set; } = null!;
    public int DurationDays { get; set; }
    public int DurationNights { get; set; }
    public decimal BasePriceAdult { get; set; }
    public decimal BasePriceChild { get; set; }
    public int MaxParticipants { get; set; }
    public int MinParticipants { get; set; }
    public string? Description { get; set; }
    public string? AdditionalInfo { get; set; }
    public string? ImportantInfo { get; set; }
    public decimal? Rating { get; set; }
    public int TotalBookings { get; set; }
    public int ViewCount { get; set; }
    public string? ImageMainUrl { get; set; }
    public List<string>? ImageUrls { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<TourItineraryDTO> Itineraries { get; set; } = [];
    public List<TourDepartureDTO> Departures { get; set; } = [];
}