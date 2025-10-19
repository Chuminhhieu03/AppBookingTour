using AutoMapper;

using AppBookingTour.Domain.Entities;
using AppBookingTour.Application.Features.TourDepartures.CreateTourDeparture;
using AppBookingTour.Application.Features.TourDepartures.GetTourDepartureById;

namespace AppBookingTour.Application.Features.TourDepartures.Mapping
{
    public class TourDepartureProfile : Profile
    {
        public TourDepartureProfile()
        {
            CreateMap<TourDepartureRequestDTO, TourDeparture>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<TourDeparture, TourDepartureDTO>();
        }
    }
}
