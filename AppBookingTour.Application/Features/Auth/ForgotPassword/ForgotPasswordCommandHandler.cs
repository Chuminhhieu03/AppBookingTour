using AppBookingTour.Application.IServices;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Text;

namespace AppBookingTour.Application.Features.Auth.ForgotPassword
{
    public class ForgotPasswordCommandHandler(
    UserManager<User> _userManager,
    IEmailService _emailService,
    ILogger<ForgotPasswordCommandHandler> _logger
) : IRequestHandler<ForgotPasswordCommand, BaseResponse>
    {
        public async Task<BaseResponse> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                // Không nên tiết lộ rằng email không tồn tại
                return new BaseResponse
                {
                    Success = true,
                    Message = "Chúng tôi đã gửi email hướng dẫn lấy lại mật khẩu qua email, bạn vui lòng kiểm tra email"
                };
            }

            // Tạo token reset
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Encode token để truyền qua URL
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            // Tạo link reset
            var resetLink = $"https://your-frontend-domain.com/reset-password?email={request.Email}&token={encodedToken}";

            // Tạo nội dung email (template)
            var html = $"<p>Xin chào {user.FullName},</p>" +
                       $"<p>Vui lòng <a href=\"{resetLink}\">bấm vào đây</a> để xác nhận cập nhật mật khẩu mới cho bạn.</p>";

            // Gửi email 
            await _emailService.SendEmailConfirmAsync(request.Email, "Lấy lại mật khẩu", html);
 
            _logger.LogInformation("Hệ thống thực hiện gửi email quên mật khẩu cho {Email}", request.Email);

            return new BaseResponse
            {
                Success = true,
                Message = "Chúng tôi đã gửi email hướng dẫn lấy lại mật khẩu qua email, bạn vui lòng kiểm tra email"
            };
        }
    }
}
