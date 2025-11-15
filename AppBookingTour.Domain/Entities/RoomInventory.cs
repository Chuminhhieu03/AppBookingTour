using System.Text.Json.Serialization;

namespace AppBookingTour.Domain.Entities;

public class RoomInventory : BaseEntity
{
    public int RoomTypeId { get; set; }
    public DateTime Date { get; set; }
    public int BookedRooms { get; set; }
    public decimal BasePriceAdult { get; set; }
    public decimal BasePriceChildren { get; set; }

    // Navigation properties
    [JsonIgnore]
    public virtual RoomType RoomType { get; set; } = null!;
}