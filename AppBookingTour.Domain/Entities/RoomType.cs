using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Domain.Entities;

public class RoomType : BaseEntity
{
    public int HotelId { get; set; }
    public string Name { get; set; } = null!;
    public int MaxOccupancy { get; set; }
    public int? BedCount { get; set; } // Số giường 
    public RoomStatus Status { get; set; } = RoomStatus.Draft;
    public decimal BasePriceAdult { get; set; }
    public decimal BasePriceChildren { get; set; }
    public string? Amenities { get; set; } // JSON

    // Navigation properties
    public virtual Accommodation Hotel { get; set; } = null!;
    public virtual ICollection<RoomInventory> RoomInventories { get; set; } = [];
}
