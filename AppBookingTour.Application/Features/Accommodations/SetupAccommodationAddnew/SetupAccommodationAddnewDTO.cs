using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.Accommodations.SetupAccommodationAddnew
{
    public class SetupAccommodationAddnewDTO : BaseResponse
    {
        public List<KeyValuePair<int, string>>? ListStatus { get; set; }
        public List<KeyValuePair<int, string>>? ListType { get; set; }
        public List<City>? ListCity { get; set; }
    }
}
