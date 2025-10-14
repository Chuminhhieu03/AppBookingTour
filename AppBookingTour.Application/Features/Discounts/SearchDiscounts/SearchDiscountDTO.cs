using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.Discounts.SearchDiscounts
{
    public class SearchDiscountResponse : BaseResponse
    {
        public List<Discount>? ListDiscount { get; set; }
    }

    public class SearchDiscountFilter
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public int? Status { get; set; }
    }
}
