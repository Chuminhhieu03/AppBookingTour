using AppBookingTour.Application.Features.Tours.GetTourById;
using MediatR;

namespace AppBookingTour.Application.Features.Tours.CreateTour;

public record CreateTourCommand(TourRequestDTO TourRequest) : IRequest<GetTourByIdResponse>;