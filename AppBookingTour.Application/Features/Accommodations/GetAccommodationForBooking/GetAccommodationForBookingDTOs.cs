namespace AppBookingTour.Application.Features.Accommodations.GetAccommodationForBooking;

/// <summary>
/// Response DTO for GetAccommodationForBooking - Contains accommodation, room type, and selected room inventories
/// </summary>
public class AccommodationForBookingDTO
{
    // Accommodation parent information (ông)
    public AccommodationInfoDTO Accommodation { get; set; } = null!;
    
    // Room type information (cha)
    public RoomTypeInfoDTO RoomType { get; set; } = null!;
    
    // Selected room inventories (con)
    public List<RoomInventoryInfoDTO> RoomInventories { get; set; } = [];
    
    // Pricing summary
    public decimal TotalPrice { get; set; }
    public int NumberOfNights { get; set; }
}

/// <summary>
/// Accommodation information
/// </summary>
public class AccommodationInfoDTO
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Address { get; set; }
    public string? CoverImgUrl { get; set; }
    public int? StarRating { get; set; }
    public decimal Rating { get; set; }
    public string? CityName { get; set; }
    public string? Description { get; set; }
}

/// <summary>
/// Room type information (for booking form display)
/// </summary>
public class RoomTypeInfoDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal? Price { get; set; }
    public int? MaxAdult { get; set; }      // For booking form validation
    public int? MaxChildren { get; set; }   // For booking form validation
    public int? Quantity { get; set; }
    public string? CoverImageUrl { get; set; }
}

/// <summary>
/// Room inventory information (specific date with pricing)
/// </summary>
public class RoomInventoryInfoDTO
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public decimal BasePriceAdult { get; set; }
    public decimal BasePriceChildren { get; set; }
    public decimal BasePrice { get; set; }
    public int AvailableRooms { get; set; }
    public string Status => AvailableRooms > 0 ? "Available" : "Full";
}
