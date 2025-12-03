using AppBookingTour.Application.Features.Tours.SearchTours;

namespace AppBookingTour.Application.Features.Accommodations.SearchAccommodationsForCustomer;

public class SearchAccommodationsForCustomerFilter
{
    public decimal? PriceFrom { get; set; }
    public decimal? PriceTo { get; set; }
    public int? CityId { get; set; }
    public int? Type { get; set; }
    public int? StarRating { get; set; }
    public DateTime? CheckInDate { get; set; }
    public DateTime? CheckOutDate { get; set; }
    public int? NumOfAdult { get; set; } = 1;
    public int? NumOfChild { get; set; } = 0;
    public int? NumOfRoom { get; set; } = 1;
}

public class SearchAccommodationsForCustomerResponse
{
    public List<CustomerAccommodationListItem> Accommodations { get; set; } = new List<CustomerAccommodationListItem>();
    public PaginationMeta Meta { get; set; } = null!;
}

public class CustomerAccommodationListItem
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string Name { get; set; } = null!;
    public int? Type { get; set; }
    public string? Address { get; set; }
    public int StarRating { get; set; }
    public decimal? Rating { get; set; }
    public string? CoverImgUrl { get; set; }
    public string CityName { get; set; } = "N/A";
    public string? Amenities { get; set; }
    public int TotalAvailableRooms { get; set; }
    public decimal MinRoomTypePrice { get; set; }
    public string AmenitiesName { get; set; }
}