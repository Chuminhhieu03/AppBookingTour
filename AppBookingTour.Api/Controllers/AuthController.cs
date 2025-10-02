using AppBookingTour.Api.Contracts.Responses;
using AppBookingTour.Application.Features.Auth.Login;
using AppBookingTour.Application.Features.Auth.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppBookingTour.Api.Controllers;

/// <summary>
/// Authentication controller following Clean Architecture
/// Uses MediatR for handling authentication use cases
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// User login endpoint
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            
            if (result.Success)
            {
                return Ok(result);
            }
            
            return Unauthorized(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for email: {Email}", command.Email);
            return BadRequest(new { Success = false, Message = "An error occurred during login" });
        }
    }

    /// <summary>
    /// User registration endpoint
    /// </summary>
    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<RegisterCommandResponse>>> Register([FromBody] RegisterCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            
            if (result.Success)
            {
                return Ok(ApiResponse<RegisterCommandResponse>.Ok(result));
            }
            
            return BadRequest(ApiResponse<RegisterCommandResponse>.Fail(result.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for email: {Email}", command.Email);
            return BadRequest(ApiResponse<RegisterCommandResponse>.Fail("Đang có lỗi xảy ra trong quá trình đăng ký, vui lòng thử lại sau"));
        }
    }

    /// <summary>
    /// Get current user profile
    /// </summary>
    //[HttpGet("profile")]
    //public async Task<ActionResult> GetProfile()
    //{
    //    try
    //    {
    //        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //        if (string.IsNullOrEmpty(userId))
    //        {
    //            return Unauthorized();
    //        }

    //        var query = new GetProfileQuery(userId);
    //        var result = await _mediator.Send(query);
            
    //        if (result == null)
    //        {
    //            return NotFound(new { Message = "User profile not found" });
    //        }

    //        return Ok(result);
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "Error getting user profile");
    //        return BadRequest(new { Message = "An error occurred while getting profile" });
    //    }
    //}

    /// <summary>
    /// User logout endpoint
    /// </summary>
    //[HttpPost("logout")]
    //public async Task<ActionResult> Logout()
    //{
    //    try
    //    {
    //        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //        if (string.IsNullOrEmpty(userId))
    //        {
    //            return Unauthorized();
    //        }

    //        // Get auth service directly from DI (for logout operations)
    //        var authService = HttpContext.RequestServices.GetRequiredService<IAuthService>();
    //        await authService.LogoutAsync(userId);
            
    //        return Ok(new { Success = true, Message = "Logged out successfully" });
    //    }
    //    catch (Exception ex)
    //    {
    //        _logger.LogError(ex, "Error during logout");
    //        return BadRequest(new { Success = false, Message = "An error occurred during logout" });
    //    }
    //}
}