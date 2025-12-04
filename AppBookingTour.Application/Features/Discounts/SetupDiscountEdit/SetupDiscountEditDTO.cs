using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.Discounts.SetupDiscountEdit
{
    public class SetupDiscountEditDTO : BaseResponse
    {
        public Discount? Discount { get; set; }
        public List<KeyValuePair<int, string>>? ListStatus { get; set; }
        public List<KeyValuePair<int, string>>? ListServiceType { get; set; }
    }
}
