using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Domain.Entities;

public class PromotionItem : BaseEntity
{
    public int PromotionId { get; set; }
    public int ItemId { get; set; } // FK to Tour, Hotel, or Combo
    public ItemType ItemType { get; set; }

    // Navigation properties
    public virtual Promotion Promotion { get; set; } = null!;
}
