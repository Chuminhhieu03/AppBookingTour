
namespace AppBookingTour.Application.Features.TourDepartures.UpdateTourDeparture;

public class UpdateTourDepartureResponse
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public static UpdateTourDepartureResponse Success() =>
        new() { IsSuccess = true };
    public static UpdateTourDepartureResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, ErrorMessage = errorMessage };
}