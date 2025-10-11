using AppBookingTour.Application.IServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Auth.Login;

/// <summary>
/// Login command handler - pure use case implementation
/// </summary>
public class LoginCommandHandler(
    IAuthService authService,
    ILogger<LoginCommandHandler> logger
) : IRequestHandler<LoginCommand, LoginCommandResponse>
{
    public async Task<LoginCommandResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Đang thực hiện login cho {Email}", request.Email);
        return await authService.LoginAsync(request);
    }
}