using AppBookingTour.Application.Features.TourItineraries.CreateTourItinerary;
using FluentValidation;

namespace AppBookingTour.Application.Features.TourItineraries.UpdateTourItinerary;

public class UpdateTourItineraryCommandValidator : AbstractValidator<UpdateTourItineraryCommand>
{
    public UpdateTourItineraryCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.TourItineraryRequest)
            .SetValidator(new TourItineraryRequestValidator());
    }
}
