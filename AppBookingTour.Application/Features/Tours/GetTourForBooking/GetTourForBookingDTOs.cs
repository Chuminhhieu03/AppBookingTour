namespace AppBookingTour.Application.Features.Tours.GetTourForBooking;

/// <summary>
/// Response DTO for GetTourForBooking - Contains tour parent info and selected departure
/// </summary>
public class TourForBookingDTO
{
    // Tour parent information (for summary card)
    public TourInfoDTO Tour { get; set; } = null!;
    
    // Selected departure information (for booking form)
    public TourDepartureInfoDTO Departure { get; set; } = null!;
}

/// <summary>
/// Tour parent information
/// </summary>
public class TourInfoDTO
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? ImageMainUrl { get; set; }
    public int DurationDays { get; set; }
    public int DurationNights { get; set; }
    public string? Description { get; set; }
    public decimal Rating { get; set; }
    public int TotalBookings { get; set; }
    public string DepartureCityName { get; set; } = null!;
    public string DestinationCityName { get; set; } = null!;
}

/// <summary>
/// Selected tour departure information
/// </summary>
public class TourDepartureInfoDTO
{
    public int Id { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public decimal PriceAdult { get; set; }
    public decimal PriceChildren { get; set; }
    public decimal SingleRoomSurcharge { get; set; }
    public int AvailableSlots { get; set; }
    public int BookedSlots { get; set; }
    public string Status { get; set; } = null!;
    public string? GuideName { get; set; }
}
