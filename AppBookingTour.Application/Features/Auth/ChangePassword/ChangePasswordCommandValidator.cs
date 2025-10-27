using FluentValidation;

namespace AppBookingTour.Application.Features.Auth.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email là b?t bu?c")
            .EmailAddress().WithMessage("Email không h?p l?");

        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("M?t kh?u hi?n t?i là b?t bu?c")
            .MinimumLength(6).WithMessage("M?t kh?u hi?n t?i ph?i có ít nh?t 6 ký t?");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("M?t kh?u m?i là b?t bu?c")
            .MinimumLength(6).WithMessage("M?t kh?u m?i ph?i có ít nh?t 6 ký t?")
            .NotEqual(x => x.CurrentPassword).WithMessage("M?t kh?u m?i ph?i khác m?t kh?u hi?n t?i");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Xác nh?n m?t kh?u là b?t bu?c")
            .Equal(x => x.NewPassword).WithMessage("Xác nh?n m?t kh?u không kh?p v?i m?t kh?u m?i");
    }
}
