
using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Application.Features.TourTypes.GetTourTypeById;

public class GetTourTypeByIdResponse
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public TourTypeDTO? TourType { get; init; }
    public static GetTourTypeByIdResponse Success(TourTypeDTO tourType) =>
        new() { IsSuccess = true, TourType = tourType };
    public static GetTourTypeByIdResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, ErrorMessage = errorMessage };
}

public class TourTypeDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public PriceLevel? PriceLevel { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}