using AppBookingTour.Api.Contracts.Responses;
using AppBookingTour.Application.Features.Auth.ConfirmEmail;
using AppBookingTour.Application.Features.Auth.ForgotPassword;
using AppBookingTour.Application.Features.Auth.Login;
using AppBookingTour.Application.Features.Auth.RefreshToken;
using AppBookingTour.Application.Features.Auth.Register;
using AppBookingTour.Application.Features.Auth.ResetPassword;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

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
    public async Task<ActionResult<ApiResponse<object>>> Login([FromBody] LoginCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return Unauthorized(result);
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
        catch (ValidationException vex)
        {
            var messages = string.Join("; ", vex.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
            _logger.LogWarning(vex, "Validation failed for login: {Errors}", messages);
            return BadRequest(ApiResponse<object>.Fail(messages));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Có lỗi xảy ra trong quá trình đăng nhập cho: {Email}", command.Email);
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
    /// User refresh token endpoint
    /// </summary>
    [HttpPost("refresh-token")]
    public async Task<ActionResult<ApiResponse<object>>> RefreshToken()
    {
        try
        {
            var refreshToken = Request.Cookies["refreshToken"];

            // Kiểm tra xem refresh token có tồn tại không 
            if (string.IsNullOrEmpty(refreshToken)){
                return Unauthorized(ApiResponse<object>.Fail("Xác thực refresh token thất bại, vui lòng đăng nhập lại"));
            }

            var result = await _mediator.Send(new RefreshTokenCommand(refreshToken));
            // Nếu không thành công 
            if(!result.Success)
            {
                return Unauthorized(ApiResponse<object>.Fail(result.Message));
            }

            // Nếu thành công thì sẽ gắn refresh token vào HTTP secure tiếp 
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
        catch(Exception ex)
        {
            _logger.LogError(ex, "Có lỗi xảy ra trong quá trình xử lý refresh token");
            return BadRequest(ApiResponse<object>.Fail("Có lỗi xảy ra trong quá trình xác thực người dùng, vui lòng đăng nhập lại"));
        }
    }

    /// <summary>
    /// User forgot password endpoint
    /// </summary>
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// User reset password endpoint
    /// </summary>
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    /// <summary>
    /// Confirm email endpoint
    /// </summary>
    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userName, [FromQuery] string token)
    {
        var result = await _mediator.Send(new ConfirmEmailCommand(userName, token));
        if (!result.Success)
        {
            return BadRequest(ApiResponse<object>.Fail(result.Message));
        }
        return Ok(ApiResponse<object>.Ok(new { result.Message }));
    }
}