using FluentValidation;

namespace AppBookingTour.Application.Features.Tours.GetTourById;

public class GetTourByIdQueryValidator : AbstractValidator<GetTourByIdQuery>
{
    public GetTourByIdQueryValidator()
    {
        RuleFor(x => x.TourId)
            .GreaterThan(0).WithMessage("Tour ID must be greater than 0");
    }
}