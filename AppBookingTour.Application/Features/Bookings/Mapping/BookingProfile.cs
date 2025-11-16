using AppBookingTour.Application.Features.Bookings.Common;
using AppBookingTour.Domain.Entities;
using AutoMapper;

namespace AppBookingTour.Application.Features.Bookings.Mapping;

public class BookingProfile : Profile
{
    public BookingProfile()
    {
        CreateMap<BookingParticipant, BookingParticipantDTO>().ReverseMap();

        CreateMap<Booking, BookingDetailsDTO>()
            .ForMember(dest => dest.PaymentDeadline, opt => opt.MapFrom(src => src.CreatedAt.AddMinutes(15)))
            .ForMember(dest => dest.PaidAmount, opt => opt.Ignore())
            .ForMember(dest => dest.RemainingAmount, opt => opt.Ignore())
            .ForMember(dest => dest.TourName, opt => opt.Ignore())
            .ForMember(dest => dest.TourImageUrl, opt => opt.Ignore())
            .ForMember(dest => dest.DepartureDate, opt => opt.Ignore())
            .ForMember(dest => dest.ReturnDate, opt => opt.Ignore())
            .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants));
    }
}
