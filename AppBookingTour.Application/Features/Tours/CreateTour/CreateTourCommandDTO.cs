using Microsoft.AspNetCore.Http;
using AppBookingTour.Application.Features.TourItineraries.CreateTourItinerary;
using AppBookingTour.Application.Features.TourDepartures.CreateTourDeparture;

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
    public string? AdditionalInfo { get; set; }
    public string? ImportantInfo { get; set; }
    public string? ItinerariesJson { get; set; }
    public string? DeparturesJson { get; set; }

    public List<TourItineraryRequestDTO>? Itineraries { get; set; } = new List<TourItineraryRequestDTO>();
    public List<TourDepartureRequestDTO>? Departures { get; set; } = new List<TourDepartureRequestDTO>();
}