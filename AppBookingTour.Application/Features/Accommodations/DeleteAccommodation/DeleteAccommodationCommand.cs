using MediatR;

namespace AppBookingTour.Application.Features.Accommodations.DeleteAccommodation;

public sealed record DeleteAccommodationCommand(int Id) : IRequest<DeleteAccommodationResponse>;


