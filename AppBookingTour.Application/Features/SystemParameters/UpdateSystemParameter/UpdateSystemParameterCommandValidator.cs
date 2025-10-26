using FluentValidation;

namespace AppBookingTour.Application.Features.SystemParameters.UpdateSystemParameter
{
    public class UpdateSystemParameterCommandValidator : AbstractValidator<UpdateSystemParameterCommand>
    {
        public UpdateSystemParameterCommandValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.RequestDto.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.RequestDto.Code)
                .MaximumLength(100).WithMessage("Code must not exceed 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.RequestDto.Code));

            RuleFor(x => x.RequestDto.Description)
                .MaximumLength(200).WithMessage("Description must not exceed 200 characters.")
                 .When(x => !string.IsNullOrEmpty(x.RequestDto.Description));

            RuleFor(x => x.RequestDto.FeatureCode)
                .NotEmpty().WithMessage("FeatureCode is required.")
                .GreaterThan(0).WithMessage("FeatureCode must be greater than 0.");
        }
    }
}