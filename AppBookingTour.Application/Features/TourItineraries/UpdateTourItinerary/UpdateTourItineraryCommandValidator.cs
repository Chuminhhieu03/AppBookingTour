using FluentValidation;

namespace AppBookingTour.Application.Features.TourItineraries.UpdateTourItinerary;

public class UpdateTourItineraryCommandValidator : AbstractValidator<UpdateTourItineraryCommand>
{
    public UpdateTourItineraryCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.TourItineraryRequest.TourId)
            .NotNull().WithMessage("TourId is required")
            .GreaterThan(0).WithMessage("TourId must be greater than 0");

        RuleFor(x => x.TourItineraryRequest.DayNumber)
            .NotNull().WithMessage("DayNumber is required")
            .GreaterThan(0).WithMessage("DayNumber must be greater than 0");

        RuleFor(x => x.TourItineraryRequest.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");
    }
}
