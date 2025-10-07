using FluentValidation;

namespace AppBookingTour.Application.Features.Auth.ResetPassword
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email không được để trống.")
                .EmailAddress().WithMessage("Email không hợp lệ.");

            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token không được để trống.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Mật khẩu mới không được để trống.")
                .MinimumLength(8).WithMessage("Mật khẩu mới phải có ít nhất 8 ký tự.")
                .Matches(@"[A-Z]").WithMessage("Mật khẩu mới phải chứa ít nhất một chữ in hoa.")
                .Matches(@"[a-z]").WithMessage("Mật khẩu mới phải chứa ít nhất một chữ thường.")
                .Matches(@"\d").WithMessage("Mật khẩu mới phải chứa ít nhất một số.")
                .Matches(@"[\W_]").WithMessage("Mật khẩu mới phải chứa ít nhất một ký tự đặc biệt.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Xác nhận mật khẩu không được để trống.")
                .Equal(x => x.NewPassword).WithMessage("Mật khẩu xác nhận không khớp.");
        }
    }
}
