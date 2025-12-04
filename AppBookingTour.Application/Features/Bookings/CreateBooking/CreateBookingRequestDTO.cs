using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Application.Features.Bookings.CreateBooking;

public class CreateBookingRequestDTO
{
    // Contact Information
    public string ContactName { get; set; } = null!;
    public string ContactEmail { get; set; } = null!;
    public string ContactPhone { get; set; } = null!;
    public string? SpecialRequests { get; set; }
    
    // Booking Information
    public int ItemId { get; set; } // Tour/Combo/Accommodation ID
    public BookingType BookingType { get; set; } = BookingType.Tour;
    public int? TourDepartureId { get; set; } // For Tour: TourDepartureId, For Combo: ComboScheduleId
    public List<int>? RoomInventoryIds { get; set; } // For Accommodation: List of RoomInventoryId
    public DateTime TravelDate { get; set; }
    public PaymentType PaymentType { get; set; } = PaymentType.FullPayment;
    
    // Quantity Information
    public int NumAdults { get; set; }
    public int NumChildren { get; set; }
    public int NumInfants { get; set; }
    public int NumSingleRooms { get; set; }
    
    // Discount Code
    public string? DiscountCode { get; set; }
    
    // Participants
    public List<CreateBookingParticipantDTO> Participants { get; set; } = [];
    
    // User
    public int UserId { get; set; }
}

public class CreateBookingParticipantDTO
{
    public string FullName { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
    public string? IdNumber { get; set; }
    public string? Nationality { get; set; } = "Vi?t Nam";
    public bool NeedSingleRoom { get; set; }
    public ParticipantType ParticipantType { get; set; }
}
