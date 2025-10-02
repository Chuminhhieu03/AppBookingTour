using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Domain.Entities;

public class TourDeparture : BaseEntity
{
    public int TourId { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public int AvailableSlots { get; set; }
    public int BookedSlots { get; set; }
    public decimal PriceAdult { get; set; }
    public decimal PriceChildren { get; set; }
    public DepartureStatus Status { get; set; } = DepartureStatus.Available;
    public int? GuideId { get; set; }

    // Navigation properties
    public virtual Tour Tour { get; set; } = null!;
    public virtual User? Guide { get; set; }
    public virtual ICollection<Booking> Bookings { get; set; } = [];
}
