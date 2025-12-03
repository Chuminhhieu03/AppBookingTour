using System.ComponentModel.DataAnnotations.Schema;

namespace AppBookingTour.Domain.Entities
{
    public class Discount : BaseEntity
    {
        #region Primary props

        public string? Code { get; set; }
        public DateTime? StartEffectedDtg { get; set; }
        public DateTime? EndEffectedDtg { get; set; }
        public string? Name { get; set; }
        public decimal? DiscountPercent { get; set; }
        public int? TotalQuantity { get; set; }
        public int? RemainQuantity { get; set; }
        public int? ServiceType { get; set; }
        public int? Status { get; set; }
        public string? Description { get; set; }
        public decimal? MaximumDiscount { get; set; }
        public decimal? MinimumOrderAmount { get; set; }

        #endregion

        #region Extend props

        [NotMapped]
        public string? StatusName { get; set; }
        [NotMapped]
        public string? ServiceTypeName { get; set; }
        [NotMapped]
        public bool? Checked { get; set; }

        #endregion

        // Navigation properties
        public virtual ICollection<DiscountUsage> DiscountUsages { get; set; } = [];
        public virtual ICollection<ItemDiscount> ItemDiscounts { get; set; }
    }
}
