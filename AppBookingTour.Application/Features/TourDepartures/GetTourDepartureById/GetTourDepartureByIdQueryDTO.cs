
namespace AppBookingTour.Application.Features.TourDepartures.GetTourDepartureById;

public class GetTourDepartureByIdResponse
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public TourDepartureDTO? TourDeparture { get; init; }
    public static GetTourDepartureByIdResponse Success(TourDepartureDTO tourDeparture) =>
        new() { IsSuccess = true, TourDeparture = tourDeparture };
    public static GetTourDepartureByIdResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, ErrorMessage = errorMessage };
}

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
    public string Status { get; set; } = null!;
}