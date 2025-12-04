
using Microsoft.AspNetCore.Http;

namespace AppBookingTour.Application.Features.TourCategories.CreateTourCategory;

public class TourCategoryRequestDTO
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? ParentCategoryId { get; set; }
    public IFormFile? Image { get; set; }
    public bool? IsActive { get; set; }
}