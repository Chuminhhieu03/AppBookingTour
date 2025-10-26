using FluentValidation;

namespace AppBookingTour.Application.Features.ComboSchedules.UpdateComboSchedule;

public class UpdateComboScheduleCommandValidator : AbstractValidator<UpdateComboScheduleCommand>
{
    public UpdateComboScheduleCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.ComboScheduleRequest.ComboId)
            .NotNull().WithMessage("ComboId is required")
            .GreaterThan(0).WithMessage("ComboId must be greater than 0");

        RuleFor(x => x.ComboScheduleRequest.DepartureDate)
            .NotNull().WithMessage("DepartureDate is required")
            .Must(BeAValidDate).WithMessage("DepartureDate must be a valid date");

        RuleFor(x => x.ComboScheduleRequest.ReturnDate)
            .NotNull().WithMessage("ReturnDate is required")
            .Must(BeAValidDate).WithMessage("ReturnDate must be a valid date")
            .GreaterThan(x => x.ComboScheduleRequest.DepartureDate)
            .WithMessage("ReturnDate must be after DepartureDate");

        RuleFor(x => x.ComboScheduleRequest.AvailableSlots)
            .NotNull().WithMessage("AvailableSlots is required")
            .GreaterThan(0).WithMessage("AvailableSlots must be greater than 0");

        RuleFor(x => x.ComboScheduleRequest.BasePriceAdult)
            .NotNull().WithMessage("BasePriceAdult is required")
            .GreaterThan(0).WithMessage("BasePriceAdult must be greater than 0");

        RuleFor(x => x.ComboScheduleRequest.BasePriceChildren)
            .NotNull().WithMessage("BasePriceChildren is required")
            .GreaterThanOrEqualTo(0).WithMessage("BasePriceChildren must be greater than or equal to 0");

        RuleFor(x => x.ComboScheduleRequest.Status)
            .NotNull().WithMessage("Status is required")
            .InclusiveBetween(1, 3).WithMessage("Status must be a valid enum value (1 = Available, 2 = Full, 3 = Cancelled)");
    }

    private bool BeAValidDate(DateTime? date)
    {
        return date.HasValue && date.Value != default;
    }
}