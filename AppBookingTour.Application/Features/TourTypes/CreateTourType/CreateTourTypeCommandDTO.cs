using AppBookingTour.Application.Features.TourTypes.GetTourTypeById;
using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Application.Features.TourTypes.CreateTourType;

public class CreateTourTypeResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;

    public TourTypeDTO? TourType { get; set; }

    public static CreateTourTypeResponse Success(TourTypeDTO tourType) =>
        new() { IsSuccess = true, TourType = tourType };

    public static CreateTourTypeResponse Fail(string message) =>
        new() { IsSuccess = false, Message = message };
}

public class TourTypeRequestDTO
{
    public string Name { get; set; } = null!;
    public PriceLevel? PriceLevel { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool? IsActive { get; set; }
}