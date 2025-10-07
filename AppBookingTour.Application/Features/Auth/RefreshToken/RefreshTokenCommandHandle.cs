using AppBookingTour.Application.Features.Auth.Login;
using AppBookingTour.Application.IServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Auth.RefreshToken
{
    public class RefreshTokenCommandHandle(IAuthService _authService, ILogger<RefreshTokenCommandHandle> _logger) : IRequestHandler<RefreshTokenCommand, LoginCommandResponse>
    {
        public async Task<LoginCommandResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Đang thực hiện quá trình refresh token");
            return await _authService.RefreshTokenAsync(request.RefreshToken);
        }
    }
}
