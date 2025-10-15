using AppBookingTour.Application.Features.TourDepartures.GetTourDepartureById;

namespace AppBookingTour.Application.Features.TourDepartures.CreateTourDeparture;

public class CreateTourDepartureResponse
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public TourDepartureDTO? TourDeparture { get; init; }
    public static CreateTourDepartureResponse Success(TourDepartureDTO tourDeparture) =>
        new() { IsSuccess = true, TourDeparture = tourDeparture };
    public static CreateTourDepartureResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, ErrorMessage = errorMessage };
}

public class TourDepartureRequestDTO
{
    public int? TourId { get; set; }
    public DateTime? DepartureDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public int? AvailableSlots { get; set; }
    public decimal? PriceAdult { get; set; }
    public decimal? PriceChildren { get; set; }
    public int? Status { get; set; }
    public int? GuideId { get; set; }
}