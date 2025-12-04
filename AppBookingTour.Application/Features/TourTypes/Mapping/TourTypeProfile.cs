using AppBookingTour.Application.Features.TourTypes.CreateTourType;
using AppBookingTour.Application.Features.TourTypes.GetTourTypeById;
using AppBookingTour.Domain.Entities;
using AutoMapper;

namespace AppBookingTour.Application.Features.TourTypes.Mapping
{
    public class TourTypeProfile : Profile
    {
        public TourTypeProfile()
        {
            CreateMap<TourTypeRequestDTO, TourType>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<TourType, TourTypeDTO>();
        }
    }
}