using MediatR;

namespace AppBookingTour.Application.Features.Tours.DeleteTour;

public record DeleteTourCommand(int TourId) : IRequest<DeleteTourCommandResponse>;
