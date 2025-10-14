using AppBookingTour.Domain.Enums;
using Microsoft.EntityFrameworkCore;

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
    [Precision(12, 2)]
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string? CoverImage { get; set; }
    [Precision(12, 2)]
    public decimal ExtraAdultPrice { get; set; }
    [Precision(12, 2)]
    public decimal ExtraChildrenPrice { get; set; }

    // Navigation properties
    public virtual Hotel Hotel { get; set; } = null!;
    public virtual ICollection<RoomInventory> RoomInventories { get; set; } = [];
}
