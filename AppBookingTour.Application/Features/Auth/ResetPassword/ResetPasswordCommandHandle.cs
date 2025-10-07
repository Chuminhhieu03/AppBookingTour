using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Text;

namespace AppBookingTour.Application.Features.Auth.ResetPassword
{
    public class ResetPasswordCommandHandler(
    UserManager<User> _userManager,
    ILogger<ResetPasswordCommandHandler> _logger
) : IRequestHandler<ResetPasswordCommand, BaseResponse>
    {
        public async Task<BaseResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Hệ thống xác nhận email thất bại, vui lòng thử lại"
                };
            }

            // Decode token (phải decode vì ta đã encode khi gửi mail)
            var decodedTokenBytes = WebEncoders.Base64UrlDecode(request.Token);
            var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);

            // Reset mật khẩu
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("Reset password thất bại với {Email}: {Errors}", request.Email, errors);
                return new BaseResponse
                {
                    Success = false,
                    Message = "Hệ thống đặt lại mật khẩu thất bại: " + errors + ". Bạn vui lòng thử lại"
                };
            }

            return new BaseResponse
            {
                Success = true,
                Message = "Mật khẩu đã được đặt lại thành công"
            };
        }
    }

}
