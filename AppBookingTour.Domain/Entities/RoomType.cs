using AppBookingTour.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AppBookingTour.Domain.Entities;

public class RoomType : BaseEntity
{
    public int AccommodationId { get; set; }
    public string Name { get; set; }
    public int? MaxAdult { get; set; }
    public int? MaxChildren { get; set; }
    public int? Status { get; set; }
    public decimal? Price { get; set; }
    public int? Quantity { get; set; }
    public string? Amenities { get; set; } // JSON
    [Precision(12, 2)]
    public string? CoverImage { get; set; }
    public decimal ExtraAdultPrice { get; set; }
    public decimal ExtraChildrenPrice { get; set; }

    // Navigation properties
    public virtual ICollection<RoomInventory> RoomInventories { get; set; } = [];
}
