namespace AppBookingTour.Domain.Entities
{
    public class ItemDiscount : BaseEntity
    {
        public int? ItemId { get; set; }
        public int? DiscountId { get; set; }
        public int? ItemType { get; set; }

        public virtual Discount Discount { get; set; }
    }
}
