using AppBookingTour.Application.Features.Auth.Login;
using MediatR;

namespace AppBookingTour.Application.Features.Auth.RefreshToken
{
    public record RefreshTokenCommand(string RefreshToken) : IRequest<LoginCommandResponse>;
}
