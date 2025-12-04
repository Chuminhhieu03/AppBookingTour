using AppBookingTour.Application.IServices;
using AppBookingTour.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;

namespace AppBookingTour.Application.Features.Auth.Register;

/// <summary>
/// Register command handler - pure use case implementation
/// </summary>
public sealed class RegisterCommandHandler(IAuthService _authService,IEmailService _emailService, ILogger<RegisterCommandHandler> _logger, IConfiguration _configuration) : IRequestHandler<RegisterCommand, RegisterCommandResponse>
{
    public async Task<RegisterCommandResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // Quá trình tạo tài khoản 
        var resultCreateUser = await _authService.RegisterAsync(request);
        // Nếu trong quá trình tạo tài khoản lỗi thì dừng tại đây và thông báo lỗi 
        if (!resultCreateUser.Success)
        {
            return resultCreateUser;
        }

        // Nếu không lỗi thì tạo token và gửi email cho user 
        try
        {
            // Gọi service để tạo token 
            var token = await _authService.GenerateTokenEmailConfirm(request.Email);

            // Encode để token nhẹ hơn để gửi qua Url
            var encode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var domain = _configuration.GetSection("FE_Domain").Value;

            var confirmationLink = $"confirm-email?userName={request.UserName}&token={encode}";

            var fullLink = $"{domain}/{confirmationLink}";

            var html = $"<p>Xin chào {request.FullName},</p>" +
                       $"<p>Vui lòng <a href=\"{fullLink}\">bấm vào đây</a> để xác nhận email của bạn.</p>";

            _logger.LogInformation("Hệ thống thực hiện gửi email xác nhận cho {Email}", request.Email);
            // Gửi mail
            await _emailService.SendEmailConfirmAsync(request.Email, "Xác nhận email", html);

            _logger.LogInformation("Hệ thống thực hiện gửi email thành công cho {Email}", request.Email);

            return new RegisterCommandResponse(true, "Đăng ký tài khoản thành công. Bạn vui lòng kiểm tra email để kích hoạt tài khoản.");

        }
        catch (Exception ex)
        {
            _logger.LogInformation("Hệ thống không thể gửi email xác nhận cho {Email}", request.Email);
            await _authService.DeleteAsync(request.Email);
            return new RegisterCommandResponse(false, $"Hệ thống không thể gửi email đến ${request.Email} trong quá trình đăng ký tài khoản, vui lòng thử lại");
        }

    }
}