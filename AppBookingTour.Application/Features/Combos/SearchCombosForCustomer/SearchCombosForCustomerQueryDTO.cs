using AppBookingTour.Application.Features.Tours.SearchTours;
using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Application.Features.Combos.SearchCombosForCustomer;

public class SearchCombosForCustomerFilter
{
    public decimal? PriceFrom { get; set; }
    public decimal? PriceTo { get; set; }
    public int? DepartureCityId { get; set; }
    public int? DestinationCityId { get; set; }
    public Vehicle? Vehicle { get; set; }
    public DateOnly? DepartureDate { get; set; }
}

public class SearchCombosForCustomerResponse
{
    public List<CustomerComboListItem> Combos { get; set; } = new List<CustomerComboListItem>();
    public PaginationMeta Meta { get; set; } = null!;
}

public class CustomerComboListItem
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int DurationDays { get; set; }
    public decimal BasePriceAdult { get; set; }
    public decimal BasePriceChildren { get; set; }
    public string FromCityName { get; set; } = "";
    public string ToCityName { get; set; } = "";
    public Vehicle Vehicle { get; set; }
    public string? ComboImageCoverUrl { get; set; }
    public decimal Rating { get; set; }
    public List<CustomerComboScheduleItem> Schedules { get; set; } = new List<CustomerComboScheduleItem>();
}

public class CustomerComboScheduleItem
{
    public int Id { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public int AvailableSlots { get; set; }
}