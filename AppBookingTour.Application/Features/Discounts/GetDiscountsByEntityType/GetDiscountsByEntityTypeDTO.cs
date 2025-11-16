using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.Discounts.GetDiscountsByEntityType
{
    public class GetDiscountsByEntityTypeResponse : BaseResponse
    {
        public List<Discount>? ListDiscount { get; set; }
    }

    public class GetDiscountsByEntityTypeFilter
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public int? Status { get; set; }
    }
}

