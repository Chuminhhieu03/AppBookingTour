//using AppBookingTour.Application.Interfaces;
//using AppBookingTour.Application.DTOs.Auth;
//using MediatR;
//using Microsoft.Extensions.Logging;

//namespace AppBookingTour.Application.Features.Auth.GetProfile;

///// <summary>
///// Get Profile use case - Query and Handler
///// </summary>

//public record GetProfileQuery(string UserId) : IRequest<UserDto?>;

//public sealed class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, UserDto?>
//{
//    private readonly IAuthService _authService;
//    private readonly ILogger<GetProfileQueryHandler> _logger;

//    public GetProfileQueryHandler(
//        IAuthService authService,
//        ILogger<GetProfileQueryHandler> logger)
//    {
//        _authService = authService;
//        _logger = logger;
//    }

//    public async Task<UserDto?> Handle(GetProfileQuery request, CancellationToken cancellationToken)
//    {
//        _logger.LogInformation("Getting profile for user: {UserId}", request.UserId);

//        try
//        {
//            var user = await _authService.GetUserByIdAsync(request.UserId);
            
//            if (user == null)
//            {
//                _logger.LogWarning("User profile not found for UserId: {UserId}", request.UserId);
//            }

//            return user;
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error occurred while getting profile for user: {UserId}", request.UserId);
//            return null;
//        }
//    }
//}