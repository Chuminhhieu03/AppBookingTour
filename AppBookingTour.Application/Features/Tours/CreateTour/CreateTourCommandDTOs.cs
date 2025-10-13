using AppBookingTour.Application.Features.Tours.GetTourById;
using AppBookingTour.Application.Features.TourItineraries.CreateTourItinerary;

namespace AppBookingTour.Application.Features.Tours.CreateTour;

public class CreateTourCommandResponse
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public TourDetailDto? Tour { get; init; }
    public static CreateTourCommandResponse Success(TourDetailDto tour) =>
        new() { IsSuccess = true, Tour = tour };
    public static CreateTourCommandResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, ErrorMessage = errorMessage };
}


public class TourRequestDTO
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public int? TypeId { get; set; }
    public int? CategoryId { get; set; }
    public int? DepartureCityId { get; set; }
    public int? DurationDays { get; set; }
    public int? DurationNights { get; set; }
    public int? MaxParticipants { get; set; }
    public int? MinParticipants { get; set; }
    public decimal? BasePriceAdult { get; set; }
    public decimal? BasePriceChild { get; set; }
    public int? Status { get; set; }
    public bool? IsActive { get; set; }
    public string? ImageGallery { get; set; }
    public string? Description { get; set; }
    public string? Includes { get; set; }
    public string? Excludes { get; set; }
    public string? TermsConditions { get; set; }
    public List<TourItineraryRequestDto>? Itineraries { get; set; }
    public List<TourDepartureRequestDTO>? TourDepartures { get; set; }
}


public class TourDepartureRequestDTO
{
    public DateTime DepartureDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public int AvailableSlots { get; set; }
    public decimal? PriceAdult { get; set; }
    public decimal? PriceChildren { get; set; }
    public int Status { get; set; }
    public int? GuideId { get; set; } 
}