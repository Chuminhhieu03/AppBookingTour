
using Microsoft.AspNetCore.Http;

namespace AppBookingTour.Application.Features.Tours.CreateTour;

public class TourCreateRequestDTO
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
    public string? Description { get; set; }
    public string? Includes { get; set; }
    public string? Excludes { get; set; }
    public string? TermsConditions { get; set; }
}