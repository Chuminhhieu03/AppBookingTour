using AppBookingTour.Application.Features.Tours.SearchTours;

namespace AppBookingTour.Application.Features.Tours.SearchToursForCustomer;

public class SearchToursForCustomerFilter
{
    public decimal? PriceFrom { get; set; }
    public decimal? PriceTo { get; set; }
    public int? DepartureCityId { get; set; }
    public int? DestinationCityId { get; set; }
    public int? TourTypeId { get; set; }
    public int? TourCategoryId { get; set; }
    public DateOnly? DepartureDate { get; set; }
}

public class SearchToursForCustomerResponse
{
    public List<CustomerTourListItem> Tours { get; set; } = new List<CustomerTourListItem>();
    public PaginationMeta Meta { get; set; } = null!;
}

public class CustomerTourListItem
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int DurationDays { get; set; }
    public int DurationNights { get; set; }
    public decimal BasePriceAdult { get; set; }
    public decimal BasePriceChild { get; set; }
    public string DepartureCityName { get; set; } = "";
    public string DestinationCityName { get; set; } = "";
    public int TypeId { get; set; }
    public string TypeName { get; set; } = "";
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = "";
    public string? ImageMainUrl { get; set; }
    public decimal Rating { get; set; }

    public List<CustomerTourDepartureItem> Departures { get; set; } = new List<CustomerTourDepartureItem>();
}

public class CustomerTourDepartureItem
{
    public int Id { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public int AvailableSlots { get; set; }
}
