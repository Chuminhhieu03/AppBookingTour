using FluentValidation;

namespace AppBookingTour.Application.Features.TourItineraries.CreateTourItinerary;

public class CreateTourItineraryCommandValidator : AbstractValidator<CreateTourItineraryCommand>
{
    public CreateTourItineraryCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.TourItineraryRequest)
            .SetValidator(new TourItineraryRequestValidator());
    }
}