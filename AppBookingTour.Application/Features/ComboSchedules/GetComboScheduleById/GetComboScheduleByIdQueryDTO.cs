namespace AppBookingTour.Application.Features.ComboSchedules.GetComboScheduleById;

public class GetComboScheduleByIdResponse
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public ComboScheduleDTO? ComboSchedule { get; init; }

    public static GetComboScheduleByIdResponse Success(ComboScheduleDTO comboSchedule) =>
        new() { IsSuccess = true, ComboSchedule = comboSchedule };

    public static GetComboScheduleByIdResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, ErrorMessage = errorMessage };
}

public class ComboScheduleDTO
{
    public int Id { get; set; }
    public int ComboId { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public int AvailableSlots { get; set; }
    public int BookedSlots { get; set; }
    public decimal BasePriceAdult { get; set; }
    public decimal BasePriceChildren { get; set; }
    public decimal SingleRoomSupplement { get; set; }
    public string Status { get; set; } = null!;
}