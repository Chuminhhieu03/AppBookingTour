using AppBookingTour.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AppBookingTour.Domain.Entities;

public class Promotion : BaseEntity
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DiscountType DiscountType { get; set; }
    public decimal DiscountValue { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public int? UsageLimit { get; set; }
    public int CurrentUsage { get; set; } = 0;
    public ApplicableTo ApplicableTo { get; set; }
    public decimal? MaximumDiscount { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Description { get; set; }

    [Precision(12, 2)]
    public decimal? MinimumDiscount{ get; set; }

    // Navigation properties
    public virtual ICollection<PromotionItem> PromotionItems { get; set; } = [];
    public virtual ICollection<PromotionUsage> PromotionUsages { get; set; } = [];
}


