using MediatR;

namespace AppBookingTour.Application.Features.Tours.GetTourById;

public record GetTourByIdQuery(int TourId) : IRequest<GetTourByIdResponse>;
