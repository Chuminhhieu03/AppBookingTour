using FluentValidation;

namespace AppBookingTour.Application.Features.Tours.CreateTour
{
    public class CreateTourCommandValidator : AbstractValidator<CreateTourCommand>
    {
        // TODO: Chỉ trả về mã lỗi => tự map sang message ở tầng API
        public CreateTourCommandValidator()
        {
            RuleFor(command => command.TourRequest.Code)
                .NotEmpty()                            
                .WithMessage("Mã tour là bắt buộc, không được để trống.");

            RuleFor(command => command.TourRequest.Name)
                .NotEmpty().WithMessage("Tên tour là bắt buộc.")
                .MaximumLength(100).WithMessage("Tên tour không được dài quá 100 ký tự.");

            RuleFor(command => command.TourRequest.DurationDays)
                .GreaterThan(0).WithMessage("Số ngày của tour phải lớn hơn 0.");
        }
    }
}