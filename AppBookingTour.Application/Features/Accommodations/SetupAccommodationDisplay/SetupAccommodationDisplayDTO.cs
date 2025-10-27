using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.Accommodations.SetupAccommodationDisplay
{
    public class SetupAccommodationDisplayDTO : BaseResponse
    {
        public Accommodation? Accommodation { get; set; }
    }
}
