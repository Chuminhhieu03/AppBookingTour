namespace AppBookingTour.Domain.Entities;

public class RoomInventory : BaseEntity
{
    public int RoomTypeId { get; set; }
    public DateTime Date { get; set; }
    public int BookedRooms { get; set; }
    public decimal BasePriceAdult { get; set; }
    public decimal BasePriceChildren { get; set; }

    // Navigation properties
    public virtual RoomType RoomType { get; set; } = null!;
}