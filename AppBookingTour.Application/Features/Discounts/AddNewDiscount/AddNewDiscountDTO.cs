using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.Discounts.AddNewDiscount
{
    public class AddNewDiscountDTO
    {
        public string? Code { get; set; }
        public DateTime? StartEffectedDtg { get; set; }
        public DateTime? EndEffectedDtg { get; set; }
        public string? Name { get; set; }
        public decimal? DiscountPercent { get; set; }
        public int? TotalQuantity { get; set; }
        public int? ServiceType { get; set; }
        public int? Status { get; set; }
        public string? Description { get; set; }
        public decimal? MaximumDiscount { get; set; }
    }

    public class AddNewDiscountResponse : BaseResponse
    {
        public Discount? Discount { get; set; }
    }
}
