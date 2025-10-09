using AppBookingTour.Share.DTOS;
using MediatR;

namespace AppBookingTour.Application.Features.Auth.ConfirmEmail;

public sealed record ConfirmEmailCommand(string UserName, string Token) : IRequest<BaseResponse>;
