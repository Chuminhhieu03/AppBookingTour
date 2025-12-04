using AppBookingTour.Share.DTOS;
using MediatR;

namespace AppBookingTour.Application.Features.Auth.ChangePassword;

public record ChangePasswordCommand(
    string Email,
    string CurrentPassword,
    string NewPassword,
    string ConfirmPassword
) : IRequest<BaseResponse>;
