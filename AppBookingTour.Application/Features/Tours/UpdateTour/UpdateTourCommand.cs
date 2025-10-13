using AppBookingTour.Application.Features.Tours.CreateTour;
using MediatR;

namespace AppBookingTour.Application.Features.Tours.UpdateTour;

public record UpdateTourCommand(int TourId, TourRequestDTO TourRequest) : IRequest<UpdateTourCommandResponse>;
