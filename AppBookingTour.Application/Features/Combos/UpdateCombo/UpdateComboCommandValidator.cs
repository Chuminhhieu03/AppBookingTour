using FluentValidation;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Application.Features.Combos.UpdateCombo;

public class UpdateComboCommandValidator : AbstractValidator<UpdateComboCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateComboCommandValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(x => x.ComboId)
            .GreaterThan(0).WithMessage("ID combo không hợp lệ");

        RuleFor(x => x.ComboRequest.Name)
            .NotEmpty().WithMessage("Tên combo không được để trống")
            .MaximumLength(200).WithMessage("Tên combo không được vượt quá 200 ký tự");

        RuleFor(x => x.ComboRequest.DurationDays)
            .GreaterThan(0).WithMessage("Số ngày du lịch phải lớn hơn 0")
            .When(x => x.ComboRequest.DurationDays.HasValue);

        RuleFor(x => x.ComboRequest.BasePriceAdult)
            .GreaterThan(0).WithMessage("Giá người lớn phải lớn hơn 0")
            .When(x => x.ComboRequest.BasePriceAdult.HasValue);

        RuleFor(x => x.ComboRequest.AdditionalInfo)
            .MaximumLength(4000).WithMessage("Thông tin thêm không được vượt quá 4000 ký tự")
            .When(x => !string.IsNullOrEmpty(x.ComboRequest.AdditionalInfo));

        RuleFor(x => x.ComboRequest.ImportantInfo)
            .MaximumLength(10000).WithMessage("Thông tin quan trọng không được vượt quá 10000 ký tự")
            .When(x => !string.IsNullOrEmpty(x.ComboRequest.ImportantInfo));
    }
}
