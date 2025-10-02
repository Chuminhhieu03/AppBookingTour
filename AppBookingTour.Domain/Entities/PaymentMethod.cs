using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Domain.Entities;

public class PaymentMethod : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public PaymentMethodType Type { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Configuration { get; set; } // JSON
    public string? Description { get; set; }

    // Navigation properties
    public virtual ICollection<Payment> Payments { get; set; } = [];
}
