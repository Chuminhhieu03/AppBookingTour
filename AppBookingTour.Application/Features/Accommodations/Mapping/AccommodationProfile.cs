using AppBookingTour.Application.Features.Accommodations.AddNewAccommodation;
using AppBookingTour.Application.Features.Accommodations.UpdateAccommodation;
using AppBookingTour.Domain.Entities;
using AutoMapper;

namespace AppBookingTour.Application.Features.Accommodations.Mapping
{
    public class AccommodationProfile : Profile
    {
        public AccommodationProfile()
        {
            CreateMap<AddNewAccommodationDTO, Accommodation>();
            CreateMap<UpdateAccommodationDTO, Accommodation>()
                .ForMember(dest => dest.ListInfoImage, opt => opt.Ignore());
        }
    }
}
