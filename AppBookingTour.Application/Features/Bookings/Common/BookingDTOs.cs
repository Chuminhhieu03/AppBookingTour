using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Application.Features.Bookings.Common;

public class BookingParticipantDTO
{
    public int? Id { get; set; }
    public string FullName { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
    public string? IdNumber { get; set; }
    public string? Nationality { get; set; }
    public bool NeedSingleRoom { get; set; }
    public ParticipantType ParticipantType { get; set; }
}

public class BookingDetailsDTO
{
    public int Id { get; set; }
    public string BookingCode { get; set; } = null!;
    public int UserId { get; set; }
    public BookingType BookingType { get; set; }
    public int ItemId { get; set; }
    public DateTime BookingDate { get; set; }
    public DateTime TravelDate { get; set; }
    public int NumAdults { get; set; }
    public int NumChildren { get; set; }
    public int NumInfants { get; set; }
    public int NumSingleRooms { get; set; }
    public decimal AdultPrice { get; set; }
    public decimal ChildPrice { get; set; }
    public decimal InfantPrice { get; set; }
    public decimal SingleRoomPrice { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalAmount { get; set; }
    public PaymentType PaymentType { get; set; }
    public BookingStatus Status { get; set; }
    public string? SpecialRequests { get; set; }
    public string ContactName { get; set; } = null!;
    public string ContactPhone { get; set; } = null!;
    public string ContactEmail { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? PaymentDeadline { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    
    // Tour Info
    public string? TourName { get; set; }
    public string? TourImageUrl { get; set; }
    public DateTime? DepartureDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    
    // Participants
    public List<BookingParticipantDTO> Participants { get; set; } = [];
    
    // Applied Discount (changed from Promotion)
    public string? DiscountCode { get; set; }
    public string? DiscountName { get; set; }
}
