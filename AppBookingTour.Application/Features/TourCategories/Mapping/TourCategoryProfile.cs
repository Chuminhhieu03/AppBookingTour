using AppBookingTour.Application.Features.TourCategories.CreateTourCategory;
using AppBookingTour.Application.Features.TourCategories.GetTourCategoryById;
using AppBookingTour.Domain.Entities;
using AutoMapper;

namespace AppBookingTour.Application.Features.TourCategories.Mapping;

public class TourCategoryProfile : Profile
{
    public TourCategoryProfile()
    {
        CreateMap<TourCategoryRequestDTO, TourCategory>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<TourCategory, TourCategoryDTO>()
            .ForMember(dest => dest.ParentCategoryName,
                       opt => opt.MapFrom(src => src.ParentCategory != null ? src.ParentCategory.Name : null));
    }
}