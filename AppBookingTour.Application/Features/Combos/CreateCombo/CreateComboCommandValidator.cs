 using FluentValidation;

namespace AppBookingTour.Application.Features.Combos.CreateCombo;

public class CreateComboCommandValidator : AbstractValidator<CreateComboCommand>
{
    public CreateComboCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.ComboRequest.Code)
            .NotEmpty().WithMessage("Code is required")
            .MaximumLength(50).WithMessage("Code must not exceed 50 characters");

        RuleFor(x => x.ComboRequest.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.ComboRequest.FromCityId)
            .NotNull().WithMessage("FromCityId is required")
            .GreaterThan(0).WithMessage("FromCityId must be greater than 0");

        RuleFor(x => x.ComboRequest.ToCityId)
            .NotNull().WithMessage("ToCityId is required")
            .GreaterThan(0).WithMessage("ToCityId must be greater than 0");

        RuleFor(x => x.ComboRequest.DurationDays)
            .NotNull().WithMessage("DurationDays is required")
            .GreaterThan(0).WithMessage("DurationDays must be greater than 0");

        RuleFor(x => x.ComboRequest.BasePriceAdult)
            .NotNull().WithMessage("BasePriceAdult is required")
            .GreaterThan(0).WithMessage("BasePriceAdult must be greater than 0");

        RuleFor(x => x.ComboRequest.Vehicle)
            .NotNull().WithMessage("Vehicle is required")
            .InclusiveBetween(1, 2).WithMessage("Vehicle must be a valid enum value (1 = Car, 2 = Plane)");
    }
}