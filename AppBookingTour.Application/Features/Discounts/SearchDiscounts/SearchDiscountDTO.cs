using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.Discounts.SearchDiscounts
{
    public class SearchDiscountResponse : BaseResponse
    {
        public List<Discount> ListDiscount { get; set; }
    }
}
