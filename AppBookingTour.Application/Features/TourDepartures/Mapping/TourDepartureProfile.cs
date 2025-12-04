using AppBookingTour.Application.Features.TourDepartures.CreateTourDeparture;
using AppBookingTour.Application.Features.TourDepartures.GetListTourDeparture;
using AppBookingTour.Application.Features.TourDepartures.GetListTourDepartureForGuide;
using AppBookingTour.Application.Features.TourDepartures.GetTourDepartureById;
using AppBookingTour.Application.Features.Tours.SearchToursForCustomer;
using AppBookingTour.Domain.Entities;
using AutoMapper;

namespace AppBookingTour.Application.Features.TourDepartures.Mapping
{
    public class TourDepartureProfile : Profile
    {
        public TourDepartureProfile()
        {
            CreateMap<TourDepartureRequestDTO, TourDeparture>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<TourDeparture, TourDepartureDTO>()
                .ForMember(dest => dest.GuideName,
                    opt => opt.MapFrom(src => src.Guide != null ? src.Guide.FullName : null));

            CreateMap<TourDeparture, ListTourDepartureItem>();

            CreateMap<TourDeparture, TourDepartureItemForGuide>()
                .ForMember(dest => dest.TourCode, opt => opt.MapFrom(src => src.Tour.Code))
                .ForMember(dest => dest.TourName, opt => opt.MapFrom(src => src.Tour.Name))
                .ForMember(dest => dest.DepartureCityName, opt => opt.MapFrom(src => src.Tour.DepartureCity.Name))
                .ForMember(dest => dest.DestinationCityName, opt => opt.MapFrom(src => src.Tour.DestinationCity.Name));

            CreateMap<TourDeparture, CustomerTourDepartureItem>();
        }
    }
}
