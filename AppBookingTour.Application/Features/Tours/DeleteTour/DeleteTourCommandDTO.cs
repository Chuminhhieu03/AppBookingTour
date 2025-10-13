namespace AppBookingTour.Application.Features.Tours.DeleteTour;

public class DeleteTourCommandResponse
{
    public bool IsSuccess { get; init; }
    public string? Message { get; init; }
    public static DeleteTourCommandResponse Success() =>
        new() { IsSuccess = true, Message = "Delete tour successfully!" }; // TODO: not hard code
    public static DeleteTourCommandResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, Message = errorMessage };
}