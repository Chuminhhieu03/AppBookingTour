using FluentValidation;

namespace AppBookingTour.Application.Features.Tours.CreateTour
{
    public class CreateTourCommandValidator : AbstractValidator<CreateTourCommand>
    {
        public CreateTourCommandValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.TourRequest.Code)
                .NotEmpty().WithMessage("Code is required")
                .MaximumLength(50).WithMessage("Code must not exceed 50 characters");

            RuleFor(x => x.TourRequest.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters");

            RuleFor(x => x.TourRequest.TypeId)
                .NotNull().WithMessage("TypeId is required")
                .GreaterThan(0).WithMessage("TypeId must be greater than 0");

            RuleFor(x => x.TourRequest.CategoryId)
                .NotNull().WithMessage("CategoryId is required")
                .GreaterThan(0).WithMessage("CategoryId must be greater than 0");

            RuleFor(x => x.TourRequest.DepartureCityId)
                .NotNull().WithMessage("DepartureCityId is required")
                .GreaterThan(0).WithMessage("DepartureCityId must be greater than 0");

            RuleFor(x => x.TourRequest.DurationDays)
                .NotNull().WithMessage("DurationDays is required")
                .GreaterThan(0).WithMessage("DurationDays must be greater than 0");

            RuleFor(x => x.TourRequest.DurationNights)
                .NotNull().WithMessage("DurationNights is required")
                .GreaterThanOrEqualTo(0).WithMessage("DurationNights must be greater than or equal to 0");

            RuleFor(x => x.TourRequest.MaxParticipants)
                .NotNull().WithMessage("MaxParticipants is required")
                .GreaterThan(0).WithMessage("MaxParticipants must be greater than 0");

            RuleFor(x => x.TourRequest.MinParticipants)
                .NotNull().WithMessage("MinParticipants is required")
                .GreaterThanOrEqualTo(1).WithMessage("MinParticipants must be at least 1")
                .LessThanOrEqualTo(x => x.TourRequest.MaxParticipants ?? int.MaxValue)
                .WithMessage("MinParticipants must not exceed MaxParticipants");

            RuleFor(x => x.TourRequest.BasePriceAdult)
                .NotNull().WithMessage("BasePriceAdult is required")
                .GreaterThan(0).WithMessage("BasePriceAdult must be greater than 0");

            RuleFor(x => x.TourRequest.BasePriceChild)
                .NotNull().WithMessage("BasePriceChild is required")
                .GreaterThanOrEqualTo(0).WithMessage("BasePriceChild must be greater than or equal to 0");

        }
    }
}
