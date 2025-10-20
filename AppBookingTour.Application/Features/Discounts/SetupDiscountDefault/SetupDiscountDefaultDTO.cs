using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.Discounts.SetupDiscountDefault
{
    public class SetupDiscountDefaultDTO : BaseResponse
    {
        public List<KeyValuePair<int, string>>? ListStatus { get; set; }
    }
}
