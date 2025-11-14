using AppBookingTour.Domain.Constants;
using FluentValidation;

namespace AppBookingTour.Application.Features.Statistics.ItemStatisticByRevenue;

public class ItemStatisticByRevenueQueryValidator : AbstractValidator<ItemStatisticByRevenueQuery>
{
    public ItemStatisticByRevenueQueryValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.ItemType)
            .IsInEnum().WithMessage("Giá trị 'ItemType' không hợp lệ.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage(string.Format(Message.RequiredField, "Ngày bắt đầu"));

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage(string.Format(Message.RequiredField, "Ngày kết thúc"))
            .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu.");
    }
}