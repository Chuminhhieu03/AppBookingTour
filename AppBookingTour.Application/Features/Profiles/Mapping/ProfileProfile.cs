using AppBookingTour.Application.Features.Profiles.GetListGuide;
using AppBookingTour.Application.Features.Profiles.GetProfileById;
using AppBookingTour.Application.Features.Profiles.UpdateProfile;
using AppBookingTour.Domain.Entities;
using AutoMapper;

namespace AppBookingTour.Application.Features.Profiles.Mapping;

public class ProfileProfile : Profile
{
    public ProfileProfile()
    {
        CreateMap<User, GetProfileByIdDTO>()
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.HasValue
                        ? DateOnly.FromDateTime(src.DateOfBirth.Value)
                        : (DateOnly?)null)
            );

        CreateMap<UpdateProfileDTO, User>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<User, GuideItemDTO>();
    }
}