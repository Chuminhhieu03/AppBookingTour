
namespace AppBookingTour.Application.Features.Tours.UpdateTour;

public class UpdateTourResponse
{
    public bool IsSuccess { get; init; }
    public string? Message { get; init; }
    public static UpdateTourResponse Success() =>
        new() { IsSuccess = true, Message = "Update tour successfully!" }; // TODO: not hard code
    public static UpdateTourResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, Message = errorMessage };
}