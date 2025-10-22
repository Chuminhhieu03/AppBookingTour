using FluentValidation;

namespace AppBookingTour.Application.Features.TourCategories.UpdateTourCategory;

public class UpdateTourCategoryCommandValidator : AbstractValidator<UpdateTourCategoryCommand>
{
    public UpdateTourCategoryCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.RequestDto.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.RequestDto.ParentCategoryId)
            .GreaterThan(0).WithMessage("ParentCategoryId must be greater than 0")
            .When(x => x.RequestDto.ParentCategoryId.HasValue);

        RuleFor(x => x)
            .Must(x => x.RequestDto.ParentCategoryId != x.TourCategoryId)
            .WithMessage("Category cannot be its own parent")
            .When(x => x.RequestDto.ParentCategoryId.HasValue);
    }
}