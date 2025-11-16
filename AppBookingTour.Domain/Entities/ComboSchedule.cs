using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Domain.Entities;

public class ComboSchedule : BaseEntity
{
    public int ComboId { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public int AvailableSlots { get; set; }
    public int BookedSlots { get; set; }
    public decimal BasePriceAdult { get; set; }
    public decimal BasePriceChildren { get; set; }
    public decimal SingleRoomSupplement { get; set; }
    public ComboStatus Status { get; set; } = ComboStatus.Available;

    // Navigation properties
    public virtual Combo Combo { get; set; } = null!;
    public virtual ICollection<Booking> Bookings { get; set; } = [];
}

