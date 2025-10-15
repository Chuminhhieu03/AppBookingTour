using FluentValidation;

namespace AppBookingTour.Application.Features.TourDepartures.UpdateTourDeparture;

public class UpdateTourDepartureCommandValidator : AbstractValidator<UpdateTourDepartureCommand>
{
    public UpdateTourDepartureCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.TourDepartureRequest.TourId)
            .NotNull().WithMessage("TourId is required")
            .GreaterThan(0).WithMessage("TourId must be greater than 0");

        RuleFor(x => x.TourDepartureRequest.DepartureDate)
            .NotNull().WithMessage("DepartureDate is required")
            .Must(BeAValidDate).WithMessage("DepartureDate must be a valid date")
            .Must(BeFutureDate).WithMessage("DepartureDate must be in the future"); ;

        RuleFor(x => x.TourDepartureRequest.ReturnDate)
            .NotNull().WithMessage("ReturnDate is required")
            .Must(BeAValidDate).WithMessage("ReturnDate must be a valid date")
            .GreaterThan(x => x.TourDepartureRequest.DepartureDate)
            .WithMessage("ReturnDate must be after DepartureDate");

        RuleFor(x => x.TourDepartureRequest.AvailableSlots)
            .NotNull().WithMessage("AvailableSlots is required")
            .GreaterThan(0).WithMessage("AvailableSlots must be greater than 0");

        RuleFor(x => x.TourDepartureRequest.PriceAdult)
            .NotNull().WithMessage("PriceAdult is required")
            .GreaterThan(0).WithMessage("PriceAdult must be greater than 0");

        RuleFor(x => x.TourDepartureRequest.PriceChildren)
            .NotNull().WithMessage("PriceChildren is required")
            .GreaterThanOrEqualTo(0).WithMessage("PriceChildren must be greater than or equal to 0");

        RuleFor(x => x.TourDepartureRequest.Status)
            .NotNull().WithMessage("Status is required")
            .InclusiveBetween(1, 3).WithMessage("Status must be a valid enum value (1 = Available, 2 = Full, 3 = Cancelled)");

        RuleFor(x => x.TourDepartureRequest.GuideId)
            .GreaterThan(0).When(x => x.TourDepartureRequest.GuideId.HasValue)
            .WithMessage("GuideId must be greater than 0 when provided");
    }

    private bool BeAValidDate(DateTime? date)
    {
        return date.HasValue && date.Value != default;
    }

    private bool BeFutureDate(DateTime? date)
    {
        return date.HasValue && date.Value.Date > DateTime.UtcNow.Date;
    }
}
