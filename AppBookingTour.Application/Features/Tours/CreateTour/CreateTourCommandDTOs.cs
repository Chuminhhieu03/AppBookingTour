using AppBookingTour.Application.Features.TourDepartures.CreateTourDeparture;
using AppBookingTour.Application.Features.TourItineraries.CreateTourItinerary;
using AppBookingTour.Application.Features.Tours.GetTourById;

namespace AppBookingTour.Application.Features.Tours.CreateTour;

public class CreateTourResponse
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public TourDTO? Tour { get; init; }
    public static CreateTourResponse Success(TourDTO tour) =>
        new() { IsSuccess = true, Tour = tour };
    public static CreateTourResponse Failed(string errorMessage) =>
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
    public bool? IsActive { get; set; }
    public string? ImageMain { get; set; } //TODO: sửa lại type => vì gửi lên sẽ là IFormFile
    public string? ImageGallery { get; set; } //TODO: sửa lại type => vì gửi lên sẽ là IFormFile[]
    public string? Description { get; set; }
    public string? Includes { get; set; }
    public string? Excludes { get; set; }
    public string? TermsConditions { get; set; }
    public List<TourItineraryRequestDTO>? Itineraries { get; set; }
    public List<TourDepartureRequestDTO>? Departures { get; set; }
}