using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Text;

namespace AppBookingTour.Application.Features.Auth.ConfirmEmail;

public sealed class ConfirmEmailCommandHandler(
    UserManager<User> _userManager,
    ILogger<ConfirmEmailCommandHandler> _logger
) : IRequestHandler<ConfirmEmailCommand, BaseResponse>
{
    public async Task<BaseResponse> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Token))
        {
            return new BaseResponse { Success = false, Message = "Thiếu userName hoặc token" };
        }

        var user = await _userManager.FindByNameAsync(request.UserName);
        if (user == null)
        {
            return new BaseResponse { Success = false, Message = "Tài khoản không tồn tại" };
        }

        string decodedToken;
        try
        {
            var decodedTokenBytes = WebEncoders.Base64UrlDecode(request.Token);
            decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);
        }
        catch
        {
            return new BaseResponse { Success = false, Message = "Token không hợp lệ" };
        }

        var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogWarning("Xác nhận email thất bại cho {UserName}: {Errors}", request.UserName, errors);
            return new BaseResponse { Success = false, Message = "Xác nhận email thất bại: " + errors };
        }

        return new BaseResponse { Success = true, Message = "Xác minh email thành công" };
    }
}
