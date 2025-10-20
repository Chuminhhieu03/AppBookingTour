using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.Discounts.SetupDiscountDisplay
{
    public class SetupDiscountDisplayDTO : BaseResponse
    {
        public Discount? Discount { get; set; }
    }
}
