using AppBookingTour.Domain.Constants;
using FluentValidation;

namespace AppBookingTour.Application.Features.TourTypes.UpdateTourType;

public class UpdateTourTypeCommandValidator : AbstractValidator<UpdateTourTypeCommand>
{
    public UpdateTourTypeCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.RequestDto.Name)
            .NotEmpty().WithMessage(string.Format(Message.RequiredField, "Tên loại tour"))
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.RequestDto.PriceLevel)
            .IsInEnum().WithMessage("Invalid PriceLevel value")
            .When(x => x.RequestDto.PriceLevel.HasValue);

        RuleFor(x => x.RequestDto.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters");
    }
}