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
            .GreaterThan(0).WithMessage("ID combo không h?p l?");

        RuleFor(x => x.ComboRequest.Name)
            .NotEmpty().WithMessage("Tên combo không ???c ?? tr?ng")
            .MaximumLength(200).WithMessage("Tên combo không ???c v??t quá 200 ký t?");

        RuleFor(x => x.ComboRequest.DurationDays)
            .GreaterThan(0).WithMessage("S? ngày du l?ch ph?i l?n h?n 0")
            .When(x => x.ComboRequest.DurationDays.HasValue);

        RuleFor(x => x.ComboRequest.BasePriceAdult)
            .GreaterThan(0).WithMessage("Giá ng??i l?n ph?i l?n h?n 0")
            .When(x => x.ComboRequest.BasePriceAdult.HasValue);
    }
}
