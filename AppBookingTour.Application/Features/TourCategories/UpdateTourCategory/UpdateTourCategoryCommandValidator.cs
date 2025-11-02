using AppBookingTour.Domain.Constants;
using FluentValidation;

namespace AppBookingTour.Application.Features.TourCategories.UpdateTourCategory;

public class UpdateTourCategoryCommandValidator : AbstractValidator<UpdateTourCategoryCommand>
{
    public UpdateTourCategoryCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.RequestDto.Name)
            .NotEmpty().WithMessage(string.Format(Message.RequiredField, "Tên danh mục"))
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.RequestDto.ParentCategoryId)
            .GreaterThan(0).WithMessage("ParentCategoryId must be greater than 0")
            .When(x => x.RequestDto.ParentCategoryId.HasValue);

        RuleFor(x => x.RequestDto.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters");
    }
}