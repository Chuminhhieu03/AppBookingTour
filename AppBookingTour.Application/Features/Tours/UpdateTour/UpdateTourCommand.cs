using AppBookingTour.Application.Features.Tours.CreateTour;
using AppBookingTour.Application.Features.Tours.GetTourById;
using MediatR;

namespace AppBookingTour.Application.Features.Tours.UpdateTour;

public record UpdateTourCommand(int TourId, TourCreateRequestDTO TourRequest) : IRequest<TourDTO>;
