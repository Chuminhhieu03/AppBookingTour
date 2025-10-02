using AppBookingTour.Domain.Enums;
using MediatR;

namespace AppBookingTour.Application.Features.Auth.Register;

// Command
public record RegisterCommand(
    string Email,
    string UserName,
    string Password,
    string ConfirmPassword,
    string FullName,
    UserType UserType,
    string? Phone = null,
    DateTime? DateOfBirth = null,
    Gender? Gender = null,
    string? Address = null
) : IRequest<RegisterCommandResponse>;
