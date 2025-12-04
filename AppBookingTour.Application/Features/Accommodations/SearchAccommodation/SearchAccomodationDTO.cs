using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.Accommodations.SearchAccommodation
{
    public class SearchAccommodationFilter
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public int? Type { get; set; }
        public int? CityId { get; set; }
    }

    public class SearchAccommodationResponse : BaseResponse
    {
        public List<Accommodation>? ListAccommodation { get; set; }
        public int? TotalCount { get; set; }
    }
}
