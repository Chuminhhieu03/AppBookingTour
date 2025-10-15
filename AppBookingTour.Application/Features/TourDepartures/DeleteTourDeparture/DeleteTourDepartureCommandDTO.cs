
namespace AppBookingTour.Application.Features.TourDepartures.DeleteTourDeparture;

public class DeleteTourDepartureResponse
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public static DeleteTourDepartureResponse Success() =>
        new() { IsSuccess = true };
    public static DeleteTourDepartureResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, ErrorMessage = errorMessage };
}