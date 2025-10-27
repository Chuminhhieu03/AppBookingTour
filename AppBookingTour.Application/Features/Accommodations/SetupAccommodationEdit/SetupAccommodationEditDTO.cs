using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.Accommodations.SetupAccommodationEdit
{
    public class SetupAccommodationEditDTO : BaseResponse
    {
        public Accommodation? Accommodation { get; set; }
        public List<KeyValuePair<int, string>>? ListStatus { get; set; }
        public List<KeyValuePair<int, string>>? ListType { get; set; }
        public List<City>? ListCity { get; set; }
    }
}
