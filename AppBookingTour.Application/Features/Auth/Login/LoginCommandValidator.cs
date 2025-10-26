using FluentValidation;

namespace AppBookingTour.Application.Features.Auth.Login;

// Validator
public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Cần phải nhập email để tiếp tục")
            .EmailAddress().WithMessage("Email không đúng định dạng");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Cần phải nhập mật khẩu để đăng nhậ")
            .MinimumLength(6).WithMessage("Mật khẩu phải có độ dài là 6 ký tự");
    }
}