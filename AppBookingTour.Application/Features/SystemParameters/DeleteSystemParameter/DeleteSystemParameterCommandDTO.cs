
namespace AppBookingTour.Application.Features.SystemParameters.DeleteSystemParameter;

public class DeleteSystemParameterResponse
{
    public bool IsSuccess { get; init; }
    public string? Message { get; init; } 

    public static DeleteSystemParameterResponse Success() =>
        new() { IsSuccess = true, Message = "System Parameter deleted successfully." }; 

    public static DeleteSystemParameterResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, Message = errorMessage };
}