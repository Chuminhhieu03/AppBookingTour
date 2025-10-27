
namespace AppBookingTour.Application.Features.SystemParameters.UpdateSystemParameter;

public class UpdateSystemParameterResponse
{
    public bool IsSuccess { get; init; }
    public string? Message { get; init; } 
    public static UpdateSystemParameterResponse Success() =>
        new() { IsSuccess = true, Message = "System Parameter updated successfully." }; 

    public static UpdateSystemParameterResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, Message = errorMessage };
}
