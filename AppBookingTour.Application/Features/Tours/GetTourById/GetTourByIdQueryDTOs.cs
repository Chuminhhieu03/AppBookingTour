using AppBookingTour.Application.Features.TourDepartures.GetTourDepartureById;
using AppBookingTour.Application.Features.TourItineraries.GetTourItineraryById;

namespace AppBookingTour.Application.Features.Tours.GetTourById;
public class GetTourByIdResponse
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public TourDTO? Tour { get; init; }

    public static GetTourByIdResponse Success(TourDTO tour) =>
        new() { IsSuccess = true, Tour = tour };

    public static GetTourByIdResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, ErrorMessage = errorMessage };
}

public class TourDTO
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int DurationDays { get; set; }
    public int DurationNights { get; set; }
    public decimal BasePriceAdult { get; set; }
    public decimal BasePriceChild { get; set; }
    public int MaxParticipants { get; set; }
    public int MinParticipants { get; set; }
    public decimal? Rating { get; set; }
    public int TotalBookings { get; set; }
    public int ViewCount { get; set; }
    public string? ImageMain { get; set; }
    public List<string> ImageGallery { get; set; } = [];
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string DepartureCityName { get; set; } = null!;
    public string TypeName { get; set; } = null!;
    public string? CategoryName { get; set; }
    public List<TourItineraryDTO> Itineraries { get; set; } = [];
    public List<TourDepartureDTO> Departures { get; set; } = [];
    public List<string> Includes { get; set; } = [];
    public List<string> Excludes { get; set; } = [];
    public string? TermsConditions { get; set; }
}