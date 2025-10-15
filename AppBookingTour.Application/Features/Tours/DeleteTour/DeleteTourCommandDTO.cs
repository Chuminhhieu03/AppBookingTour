namespace AppBookingTour.Application.Features.Tours.DeleteTour;

public class DeleteTourResponse
{
    public bool IsSuccess { get; init; }
    public string? Message { get; init; }
    public static DeleteTourResponse Success() =>
        new() { IsSuccess = true, Message = "Delete tour successfully!" }; // TODO: not hard code
    public static DeleteTourResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, Message = errorMessage };
}