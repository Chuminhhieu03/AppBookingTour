using MediatR;

namespace AppBookingTour.Application.Features.Profiles.UpdateProfile;

public record UpdateProfileCommand(int Id, UpdateProfileDTO Dto) : IRequest<Unit>;