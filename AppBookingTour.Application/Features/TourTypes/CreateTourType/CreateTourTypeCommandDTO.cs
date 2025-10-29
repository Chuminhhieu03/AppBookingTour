using AppBookingTour.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace AppBookingTour.Application.Features.TourTypes.CreateTourType;

public class TourTypeRequestDTO
{
    public string Name { get; set; } = null!;
    public PriceLevel? PriceLevel { get; set; }
    public string? Description { get; set; }
    public IFormFile? Image { get; set; }
    public bool? IsActive { get; set; }
}