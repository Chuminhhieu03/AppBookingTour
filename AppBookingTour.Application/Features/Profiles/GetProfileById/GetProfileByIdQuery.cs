using MediatR;

namespace AppBookingTour.Application.Features.Profiles.GetProfileById;

public record GetProfileByIdQuery(int Id) : IRequest<GetProfileByIdDTO>;
