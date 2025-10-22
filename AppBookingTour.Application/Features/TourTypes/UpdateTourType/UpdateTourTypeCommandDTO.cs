namespace AppBookingTour.Application.Features.TourTypes.UpdateTourType;

public class UpdateTourTypeResponse
{
    public bool IsSuccess { get; init; }
    public string? Message { get; init; }

    public static UpdateTourTypeResponse Success() =>
        new() { IsSuccess = true, Message = "Update tour type successfully!" };

    public static UpdateTourTypeResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, Message = errorMessage };
}