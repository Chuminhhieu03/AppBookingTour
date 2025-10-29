    using AppBookingTour.Api.Contracts.Responses;
using AppBookingTour.Application.Features.Auth.ChangePassword;
using AppBookingTour.Application.Features.Auth.ConfirmEmail;
using AppBookingTour.Application.Features.Auth.ForgotPassword;
using AppBookingTour.Application.Features.Auth.Login;
using AppBookingTour.Application.Features.Auth.RefreshToken;
using AppBookingTour.Application.Features.Auth.Register;
using AppBookingTour.Application.Features.Auth.ResetPassword;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppBookingTour.Api.Controllers;

/// <summary>
/// Authentication controller following Clean Architecture
/// Uses MediatR for handling authentication use cases
/// </summary>
[ApiController]
[Route("api/auth")]
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
    public async Task<ActionResult<ApiResponse<object>>> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return Unauthorized(ApiResponse<object>.Fail(result.Message));
        }

        if (!string.IsNullOrEmpty(result.RefreshToken) && result.RefreshTokenExpiry.HasValue)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = result.RefreshTokenExpiry.Value
            };

            Response.Cookies.Append("refreshToken", result.RefreshToken, cookieOptions);
        }

        return Ok(ApiResponse<object>.Ok(new
        {
            result.Token,
            result.Expiration,
            result.Message,
            result.Success
        }));
    }

    /// <summary>
    /// User registration endpoint
    /// </summary>
    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<RegisterCommandResponse>>> Register([FromBody] RegisterCommand command)
    {
        var result = await _mediator.Send(command);
        
        if (result.Success)
        {
            return Ok(ApiResponse<RegisterCommandResponse>.Ok(result));
        }
        
        return BadRequest(ApiResponse<RegisterCommandResponse>.Fail(result.Message));
    }

    /// <summary>
    /// User refresh token endpoint
    /// </summary>
    [HttpPost("refresh-token")]
    public async Task<ActionResult<ApiResponse<object>>> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized(ApiResponse<object>.Fail("Xác thực refresh token thất bại, vui lòng đăng nhập lại"));
        }

        var result = await _mediator.Send(new RefreshTokenCommand(refreshToken));
        
        if (!result.Success)
        {
            return Unauthorized(ApiResponse<object>.Fail(result.Message));
        }

        if (!string.IsNullOrEmpty(result.RefreshToken) && result.RefreshTokenExpiry.HasValue)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = result.RefreshTokenExpiry.Value
            };

            Response.Cookies.Append("refreshToken", result.RefreshToken, cookieOptions);
        }

        return Ok(ApiResponse<object>.Ok(new
        {
            result.Token,
            result.Expiration,
            result.Message,
            result.Success
        }));
    }

    /// <summary>
    /// User forgot password endpoint
    /// </summary>
    [HttpPost("forgot-password")]
    public async Task<ActionResult<ApiResponse<object>>> ForgotPassword([FromBody] ForgotPasswordCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(ApiResponse<object>.Fail(result.Message));
        }

        return Ok(ApiResponse<object>.Ok(new { result.Message }));
    }

    /// <summary>
    /// User reset password endpoint
    /// </summary>
    [HttpPost("reset-password")]
    public async Task<ActionResult<ApiResponse<object>>> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(ApiResponse<object>.Fail(result.Message));
        }

        return Ok(ApiResponse<object>.Ok(new { result.Message }));
    }

    /// <summary>
    /// Change password endpoint (requires authentication)
    /// </summary>
    [HttpPost("change-password")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<object>>> ChangePassword([FromBody] ChangePasswordCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(ApiResponse<object>.Fail(result.Message));
        }

        return Ok(ApiResponse<object>.Ok(new { result.Message }));
    }

    /// <summary>
    /// Logout endpoint (clears refresh token cookie)
    /// </summary>
    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("refreshToken");
        
        _logger.LogInformation("User logged out successfully");
        
        return Ok(ApiResponse<object>.Ok(new { Message = "Đăng xuất thành công" }));
    }

    /// <summary>
    /// Confirm email endpoint
    /// </summary>
    [HttpGet("confirm-email")]
    public async Task<ActionResult<ApiResponse<object>>> ConfirmEmail([FromQuery] string userName, [FromQuery] string token)
    {
        var result = await _mediator.Send(new ConfirmEmailCommand(userName, token));
        
        if (!result.Success)
        {
            return BadRequest(ApiResponse<object>.Fail(result.Message));
        }
        
        return Ok(ApiResponse<object>.Ok(new { result.Message }));
    }
}