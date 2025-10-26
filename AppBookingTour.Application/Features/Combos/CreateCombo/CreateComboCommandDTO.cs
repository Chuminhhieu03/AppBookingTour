using AppBookingTour.Application.Features.Combos.GetComboById;
using AppBookingTour.Application.Features.ComboSchedules.CreateComboSchedule;
using Microsoft.AspNetCore.Http;

namespace AppBookingTour.Application.Features.Combos.CreateCombo;

public class CreateComboResponse
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public ComboDTO? Combo { get; init; }

    public static CreateComboResponse Success(ComboDTO combo) =>
        new() { IsSuccess = true, Combo = combo };

    public static CreateComboResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, ErrorMessage = errorMessage };
}

public class ComboRequestDTO
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public int? FromCityId { get; set; }
    public int? ToCityId { get; set; }
    public string? ShortDescription { get; set; }
    public int? Vehicle { get; set; }
    public string? ComboImageCover { get; set; } // TODO: sửa lại type => vì gửi lên sẽ là IFormFile
    public string? ComboImages { get; set; } //TODO: sửa lại type => vì gửi lên sẽ là IFormFile[] 
    public int? DurationDays { get; set; }
    public decimal? BasePriceAdult { get; set; }
    public decimal? BasePriceChildren { get; set; }
    public string? Amenities { get; set; }
    public string? Description { get; set; }
    public string? Includes { get; set; }
    public string? Excludes { get; set; }
    public string? TermsConditions { get; set; }
    public bool? IsActive { get; set; }
    public List<ComboScheduleRequestDTO>? Schedules { get; set; }
}