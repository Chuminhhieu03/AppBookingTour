using AppBookingTour.Application.Features.Tours.GetTourById;
using MediatR;

namespace AppBookingTour.Application.Features.Tours.CreateTour;

public record CreateTourCommand(TourCreateRequestDTO TourRequest) : IRequest<TourDTO>;