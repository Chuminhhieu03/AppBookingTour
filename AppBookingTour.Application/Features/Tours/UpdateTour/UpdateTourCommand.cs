using AppBookingTour.Application.Features.Tours.GetTourById;
using MediatR;

namespace AppBookingTour.Application.Features.Tours.UpdateTour;

public record UpdateTourCommand(int TourId, TourUpdateRequestDTO TourRequest) : IRequest<TourDTO>;
