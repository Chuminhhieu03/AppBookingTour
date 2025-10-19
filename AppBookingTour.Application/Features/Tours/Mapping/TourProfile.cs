using AutoMapper;

using AppBookingTour.Application.Features.Tours.CreateTour;
using AppBookingTour.Application.Features.Tours.GetTourById;
using AppBookingTour.Domain.Entities;

namespace AppBookingTour.Application.Features.Tours.Mapping
{
    public class TourProfile : Profile
    {
        public TourProfile()
        {
            #region Tour mapping
            CreateMap<TourRequestDTO, Tour>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Tour, TourDetailDto>()
                .ForMember(dest => dest.DepartureCityName, opt => opt.MapFrom(src => src.DepartureCity.Name)) //TODO: fix khong hien thi
                .ForMember(dest => dest.TypeName, opt => opt.MapFrom(src => src.Type.Name)) //TODO: fix khong hien thi
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null)) //TODO: fix khong hien thi

                //.ForMember(dest => dest.ImageGallery, opt => opt.MapFrom(src =>
                //    string.IsNullOrEmpty(src.ImageGallery)
                //    ? new List<string>()
                //    : src.ImageGallery.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList()))

                .ForMember(dest => dest.Includes, opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src.Includes)
                    ? new List<string>()
                    : src.Includes.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList()))

                .ForMember(dest => dest.Excludes, opt => opt.MapFrom(src =>
                    string.IsNullOrEmpty(src.Excludes)
                    ? new List<string>()
                    : src.Excludes.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList()));
            #endregion
        }
    }
}
