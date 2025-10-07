using AppBookingTour.Share.DTOS;
using MediatR;

namespace AppBookingTour.Application.Features.Auth.ResetPassword
{
    public record ResetPasswordCommand(string Email, string Token, string NewPassword, string ConfirmPassword) : IRequest<BaseResponse>
    {
    }
}
