using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Auth.ChangePassword;

public class ChangePasswordCommandHandler(
    UserManager<User> _userManager,
    ILogger<ChangePasswordCommandHandler> _logger
) : IRequestHandler<ChangePasswordCommand, BaseResponse>
{
    public async Task<BaseResponse> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("B?t ??u quá trình ??i m?t kh?u cho email: {Email}", request.Email);

            // Tìm user theo email
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning("Không tìm th?y user v?i email: {Email}", request.Email);
                return new BaseResponse
                {
                    Success = false,
                    Message = "Không tìm th?y tài kho?n v?i email này"
                };
            }

            // Ki?m tra m?t kh?u hi?n t?i
            var isCurrentPasswordCorrect = await _userManager.CheckPasswordAsync(user, request.CurrentPassword);
            if (!isCurrentPasswordCorrect)
            {
                _logger.LogWarning("M?t kh?u hi?n t?i không ?úng cho email: {Email}", request.Email);
                return new BaseResponse
                {
                    Success = false,
                    Message = "M?t kh?u hi?n t?i không chính xác"
                };
            }

            // ??i m?t kh?u
            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("??i m?t kh?u th?t b?i cho {Email}: {Errors}", request.Email, errors);
                return new BaseResponse
                {
                    Success = false,
                    Message = $"??i m?t kh?u th?t b?i: {errors}"
                };
            }

            _logger.LogInformation("??i m?t kh?u thành công cho {Email}", request.Email);
            
            return new BaseResponse
            {
                Success = true,
                Message = "??i m?t kh?u thành công"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "L?i khi ??i m?t kh?u cho {Email}", request.Email);
            return new BaseResponse
            {
                Success = false,
                Message = "Có l?i x?y ra trong quá trình ??i m?t kh?u"
            };
        }
    }
}
