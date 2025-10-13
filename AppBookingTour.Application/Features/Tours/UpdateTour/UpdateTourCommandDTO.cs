
namespace AppBookingTour.Application.Features.Tours.UpdateTour;

public class UpdateTourCommandResponse
{
    public bool IsSuccess { get; init; }
    public string? Message { get; init; }
    public static UpdateTourCommandResponse Success() =>
        new() { IsSuccess = true, Message = "Update tour successfully!" }; // TODO: not hard code
    public static UpdateTourCommandResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, Message = errorMessage };
}