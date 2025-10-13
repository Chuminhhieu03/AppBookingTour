using AppBookingTour.Domain.Constants;
using FluentValidation;

namespace AppBookingTour.Application.Features.Discounts.UpdateDiscount
{
    public class UpdateDiscountValidator : AbstractValidator<UpdateDiscountCommand>
    {
        public UpdateDiscountValidator()
        {
            RuleFor(x => x.Discount)
            .SetValidator(new DiscountDtoValidator());
        }
    }

    public class DiscountDtoValidator : AbstractValidator<UpdateDiscountDTO>
    {
        public DiscountDtoValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(string.Format(Message.RequiredField, "Mã"));
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(string.Format(Message.RequiredField, "Tên mã giảm giá"));
            RuleFor(x => x.DiscountPercent)
                .NotNull().WithMessage(string.Format(Message.RequiredField, "Mức giảm"))
                .GreaterThanOrEqualTo(0).WithMessage("Mức giảm phải lớn hơn hoặc bằng 0");
            RuleFor(x => x.StartEffectedDtg)
                .NotNull().WithMessage(string.Format(Message.RequiredField, "Ngày bắt đầu hiệu lực"));
        }
    }
}
