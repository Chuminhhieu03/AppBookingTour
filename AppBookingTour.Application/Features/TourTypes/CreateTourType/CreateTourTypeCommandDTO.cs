using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Application.Features.TourTypes.CreateTourType;

public class TourTypeRequestDTO
{
    public string Name { get; set; } = null!;
    public PriceLevel? PriceLevel { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool? IsActive { get; set; }
}