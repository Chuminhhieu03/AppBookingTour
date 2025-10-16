
namespace AppBookingTour.Application.Features.ComboSchedules.UpdateComboSchedule;

public class UpdateComboScheduleResponse
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }

    public static UpdateComboScheduleResponse Success() =>
        new() { IsSuccess = true };

    public static UpdateComboScheduleResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, ErrorMessage = errorMessage };
}