using AppBookingTour.Share.DTOS;
using MediatR;

namespace AppBookingTour.Application.Features.Auth.ForgotPassword
{
    public record ForgotPasswordCommand(string Email) : IRequest<BaseResponse>
    {
    }
}
