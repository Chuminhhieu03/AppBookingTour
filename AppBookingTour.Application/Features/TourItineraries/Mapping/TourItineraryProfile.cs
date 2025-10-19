using AutoMapper;

using AppBookingTour.Domain.Entities;
using AppBookingTour.Application.Features.TourItineraries.CreateTourItinerary;
using AppBookingTour.Application.Features.TourItineraries.GetTourItineraryById;

namespace AppBookingTour.Application.Features.TourItineraries.Mapping
{
    public class TourItineraryProfile : Profile
    {
        public TourItineraryProfile()
        {
            #region TourItinerary mapping
            CreateMap<TourItineraryRequestDTO, TourItinerary>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<TourItinerary, TourItineraryDTO>();
            #endregion
        }
    }
}
