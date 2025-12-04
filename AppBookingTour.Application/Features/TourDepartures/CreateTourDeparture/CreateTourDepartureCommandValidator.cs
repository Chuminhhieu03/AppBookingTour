using FluentValidation;

namespace AppBookingTour.Application.Features.TourDepartures.CreateTourDeparture;

public class CreateTourDepartureCommandValidator : AbstractValidator<CreateTourDepartureCommand>
{
    public CreateTourDepartureCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.TourDepartureRequest)
            .SetValidator(new TourDepartureRequestValidator());
    }
}