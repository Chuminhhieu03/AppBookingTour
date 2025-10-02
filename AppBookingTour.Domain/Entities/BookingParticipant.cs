using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Domain.Entities;

public class BookingParticipant : BaseEntity
{
    public int BookingId { get; set; }
    public string FullName { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
    public string? IdNumber { get; set; }
    public string? Nationality { get; set; }
    public bool NeedSingleRoom { get; set; } = false;
    public ParticipantType ParticipantType { get; set; }
    // Navigation properties
    public virtual Booking Booking { get; set; } = null!;
}

