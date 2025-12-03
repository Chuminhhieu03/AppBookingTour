namespace AppBookingTour.Application.Features.ItemDiscounts.AssignDiscount;

public class AssignDiscountRequestDTO
{
    public List<int> ListDiscountId { get; set; } = new();
    public int ItemId { get; set; }
    public int ItemType { get; set; }
}

