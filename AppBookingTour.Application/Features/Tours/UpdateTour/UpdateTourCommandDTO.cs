using Microsoft.AspNetCore.Http;

namespace AppBookingTour.Application.Features.Tours.UpdateTour;

public class TourUpdateRequestDTO
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public int? TypeId { get; set; }
    public int? CategoryId { get; set; }
    public int? DepartureCityId { get; set; }
    public int? DestinationCityId { get; set; }
    public int? DurationDays { get; set; }
    public int? DurationNights { get; set; }
    public int? MaxParticipants { get; set; }
    public int? MinParticipants { get; set; }
    public decimal? BasePriceAdult { get; set; }
    public decimal? BasePriceChild { get; set; }
    public bool? IsActive { get; set; }
    public IFormFile? ImageMain { get; set; }
    public List<IFormFile>? Images { get; set; }
    public List<string>? RemoveImageUrls { get; set; }
    public string? Description { get; set; }
    public string? AdditionalInfo { get; set; }
    public string? ImportantInfo { get; set; }
}