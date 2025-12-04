
namespace AppBookingTour.Application.Features.ComboSchedules.DeleteComboSchedule;

public class DeleteComboScheduleResponse
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }

    public static DeleteComboScheduleResponse Success() =>
        new() { IsSuccess = true };

    public static DeleteComboScheduleResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, ErrorMessage = errorMessage };
}