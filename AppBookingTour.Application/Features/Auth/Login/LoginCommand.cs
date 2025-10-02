using MediatR;

namespace AppBookingTour.Application.Features.Auth.Login;

// Command
public record LoginCommand(
    string Email,
    string Password
) : IRequest<LoginCommandResponse>;
