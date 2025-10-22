using FluentValidation;

namespace AppBookingTour.Application.Features.TourTypes.CreateTourType;

public class CreateTourTypeCommandValidator : AbstractValidator<CreateTourTypeCommand>
{
    public CreateTourTypeCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.RequestDto.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.RequestDto.PriceLevel)
            .IsInEnum().WithMessage("Invalid PriceLevel value")
            .When(x => x.RequestDto.PriceLevel.HasValue);

        RuleFor(x => x.RequestDto.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters");
    }
}