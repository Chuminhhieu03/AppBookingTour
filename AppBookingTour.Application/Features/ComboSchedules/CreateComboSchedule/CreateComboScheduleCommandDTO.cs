using AppBookingTour.Application.Features.ComboSchedules.GetComboScheduleById;

namespace AppBookingTour.Application.Features.ComboSchedules.CreateComboSchedule;

public class CreateComboScheduleResponse
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public ComboScheduleDTO? ComboSchedule { get; init; }

    public static CreateComboScheduleResponse Success(ComboScheduleDTO comboSchedule) =>
        new() { IsSuccess = true, ComboSchedule = comboSchedule };

    public static CreateComboScheduleResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, ErrorMessage = errorMessage };
}

public class ComboScheduleRequestDTO
{
    public int? ComboId { get; set; }
    public DateTime? DepartureDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public int? AvailableSlots { get; set; }
    public decimal? BasePriceAdult { get; set; }
    public decimal? BasePriceChildren { get; set; }
    public int? Status { get; set; }
}