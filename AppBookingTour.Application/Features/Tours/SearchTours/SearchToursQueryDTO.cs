using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.Tours.SearchTours
{
    public class SearchTourFilter
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? CategoryId { get; set; }
        public int? TypeId { get; set; }
        public int? CityId { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public bool? Active { get; set; }
    }

    public class PaginationMeta
    {
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
    public class TourListItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string DepartureCityName { get; set; } = null!;
        public string DestinationCityName { get; set; } = null!;
        public string? ImageMainUrl { get; set; }
        public decimal BasePriceAdult { get; set; }
        public decimal BasePriceChild { get; set; }
        public int DurationDays { get; set; }
        public int DurationNights { get; set; }
        public string? TypeName { get; set; }
        public string? CategoryName { get; set; }
        public decimal Rating { get; set; }
        public int TotalBookings { get; set; }
        public bool IsActive { get; set; }
    }

    public class SearchToursResponse : BaseResponse
    {
        public List<TourListItem> Tours { get; set; } = [];

        public PaginationMeta Meta { get; set; } = null!;
    }
}