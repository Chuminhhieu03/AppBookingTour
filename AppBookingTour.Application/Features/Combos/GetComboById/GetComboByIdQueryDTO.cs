using AppBookingTour.Application.Features.ComboSchedules.GetComboScheduleById;

namespace AppBookingTour.Application.Features.Combos.GetComboById;

public class GetComboByIdResponse
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public ComboDTO? Combo { get; init; }

    public static GetComboByIdResponse Success(ComboDTO combo) =>
        new() { IsSuccess = true, Combo = combo };

    public static GetComboByIdResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, ErrorMessage = errorMessage };
}

public class ComboDTO
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string FromCityName { get; set; } = null!;
    public string ToCityName { get; set; } = null!;
    public string? ShortDescription { get; set; }
    public string Vehicle { get; set; } = null!;
    public string? ComboImageCover { get; set; }
    public List<string> HotelImages { get; set; } = [];
    public int DurationDays { get; set; }
    public decimal BasePriceAdult { get; set; }
    public decimal BasePriceChildren { get; set; }
    public List<string> Amenities { get; set; } = [];
    public string? Description { get; set; }
    public List<string> Includes { get; set; } = [];
    public List<string> Excludes { get; set; } = [];
    public string? TermsConditions { get; set; }
    public decimal Rating { get; set; }
    public int TotalBookings { get; set; }
    public int ViewCount { get; set; }
    public bool IsActive { get; set; }
    public List<ComboScheduleDTO> Schedules { get; set; } = [];
}