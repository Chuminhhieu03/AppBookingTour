namespace AppBookingTour.Application.Features.TourTypes.DeleteTourType;

public class DeleteTourTypeResponse
{
    public bool IsSuccess { get; init; }
    public string? Message { get; init; }

    public static DeleteTourTypeResponse Success() =>
        new() { IsSuccess = true, Message = "Delete tour type successfully!" };

    public static DeleteTourTypeResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, Message = errorMessage };
}