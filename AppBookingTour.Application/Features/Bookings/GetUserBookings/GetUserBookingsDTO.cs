using AppBookingTour.Application.Features.Tours.SearchTours;
using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Application.Features.Bookings.GetUserBookings;

public class GetUserBookingsFilter
{
    public BookingStatus? Status { get; set; }
}

public class GetUserBookingsResponse
{
    public List<UserBookingItem> Bookings { get; set; } = new List<UserBookingItem>();
    public PaginationMeta Meta { get; set; } = null!;
}

public class UserBookingItem
{
    public int Id { get; set; }
    public string BookingCode { get; set; } = null!;
    public BookingType BookingType { get; set; }
    public string BookingTypeName { get; set; } = "";
    public DateTime BookingDate { get; set; }
    public DateTime TravelDate { get; set; }
    public int NumAdults { get; set; }
    public int NumChildren { get; set; }
    public int NumInfants { get; set; }
    public decimal FinalAmount { get; set; }
    public BookingStatus Status { get; set; }
    public string StatusName { get; set; } = "";
    public string ContactName { get; set; } = null!;
    public string ContactPhone { get; set; } = null!;
    
    // Item details based on BookingType
    public BookingItemDetails? ItemDetails { get; set; }
}

public class BookingItemDetails
{
    public int ItemId { get; set; }
    public string? ItemCode { get; set; }
    public string? ItemName { get; set; }
    public string? ItemImageUrl { get; set; }
    public int? DurationDays { get; set; }
    public int? DurationNights { get; set; }
    
    // For Tour
    public string? DepartureCityName { get; set; }
    public string? DestinationCityName { get; set; }
    
    // For Combo
    public string? FromCityName { get; set; }
    public string? ToCityName { get; set; }
    public string? Vehicle { get; set; }
    
    // For Accommodation
    public string? AccommodationCityName { get; set; }
    public string? AccommodationType { get; set; }
}
