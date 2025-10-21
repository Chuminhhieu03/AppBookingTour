using AppBookingTour.Application.Features.Cities.GetCityById;
using AppBookingTour.Domain.Entities;
using AutoMapper;

namespace AppBookingTour.Application.Features.Cities.Mapping;

public class CityProfile : Profile
{
    public CityProfile()
    {
        CreateMap<City, CityDTO>()
            .ForMember(dest => dest.RegionName, opt => opt.Ignore());
    }
}