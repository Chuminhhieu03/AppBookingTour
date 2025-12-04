using AppBookingTour.Application.Features.Tours.CreateTour;
using AppBookingTour.Application.Features.Tours.GetTourById;
using AppBookingTour.Application.Features.Tours.SearchTours;
using AppBookingTour.Application.Features.Tours.SearchToursForCustomer;
using AppBookingTour.Application.Features.Tours.UpdateTour;
using AppBookingTour.Domain.Entities;
using AutoMapper;

namespace AppBookingTour.Application.Features.Tours.Mapping
{
    public class TourProfile : Profile
    {
        public TourProfile()
        {
            #region Tour mapping
            CreateMap<TourCreateRequestDTO, Tour>()
                .ForMember(dest => dest.ImageMainUrl, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<TourUpdateRequestDTO, Tour>()
               .ForMember(dest => dest.ImageMainUrl, opt => opt.Ignore())
               .ForMember(dest => dest.Images, opt => opt.Ignore())
               .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Tour, TourDTO>()
                .ForMember(dest => dest.DepartureCityName, opt => opt.MapFrom(src => src.DepartureCity.Name))
                .ForMember(dest => dest.DestinationCityName, opt => opt.MapFrom(src => src.DestinationCity.Name))
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));

            CreateMap<Tour, TourListItem>()
                .ForMember(dest => dest.DepartureCityName, opt => opt.MapFrom(src => src.DepartureCity != null ? src.DepartureCity.Name : "N/A"))
                .ForMember(dest => dest.DestinationCityName, opt => opt.MapFrom(src => src.DestinationCity != null ? src.DestinationCity.Name : "N/A"))
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type != null ? src.Type.Name : "N/A"))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : "N/A"));

            CreateMap<Tour, CustomerTourListItem>()
                .ForMember(dest => dest.DepartureCityName, opt => opt.MapFrom(src => src.DepartureCity != null ? src.DepartureCity.Name : "N/A"))
                .ForMember(dest => dest.DestinationCityName, opt => opt.MapFrom(src => src.DestinationCity != null ? src.DestinationCity.Name : "N/A"))
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type != null ? src.Type.Name : "N/A"))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : "N/A"));

            #endregion
        }
    }
}
