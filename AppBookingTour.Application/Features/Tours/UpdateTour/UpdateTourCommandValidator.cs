using AppBookingTour.Domain.Constants;
using FluentValidation;

namespace AppBookingTour.Application.Features.Tours.CreateTour
{
    public class UpdateTourCommandValidator : AbstractValidator<CreateTourCommand>
    {
        public UpdateTourCommandValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.TourRequest.Code)
                .NotEmpty().WithMessage(string.Format(Message.RequiredField, "Mã tour"))
                .MaximumLength(50).WithMessage("Code must not exceed 50 characters");

            RuleFor(x => x.TourRequest.Name)
                .NotEmpty().WithMessage(string.Format(Message.RequiredField, "Tên tour"))
                .MaximumLength(200).WithMessage("Name must not exceed 200 characters");

            RuleFor(x => x.TourRequest.TypeId)
                .NotNull().WithMessage(string.Format(Message.RequiredField, "Loại tour"))
                .GreaterThan(0).WithMessage("TypeId must be greater than 0");

            RuleFor(x => x.TourRequest.CategoryId)
                .NotNull().WithMessage(string.Format(Message.RequiredField, "Danh mục tour"))
                .GreaterThan(0).WithMessage("CategoryId must be greater than 0");

            RuleFor(x => x.TourRequest.DepartureCityId)
                .NotNull().WithMessage(string.Format(Message.RequiredField, "Thành phố khởi hành"))
                .GreaterThan(0).WithMessage("DepartureCityId must be greater than 0");

            RuleFor(x => x.TourRequest.DurationDays)
                .NotNull().WithMessage(string.Format(Message.RequiredField, "Số ngày lưu trú"))
                .GreaterThan(0).WithMessage("DurationDays must be greater than 0");

            RuleFor(x => x.TourRequest.DurationNights)
                .NotNull().WithMessage(string.Format(Message.RequiredField, "Số đêm lưu trú"))
                .GreaterThanOrEqualTo(0).WithMessage("DurationNights must be greater than or equal to 0");

            RuleFor(x => x.TourRequest.MaxParticipants)
                .NotNull().WithMessage(string.Format(Message.RequiredField, "Số lượng hành khách tối đa"))
                .GreaterThan(0).WithMessage("MaxParticipants must be greater than 0");

            RuleFor(x => x.TourRequest.MinParticipants)
                .NotNull().WithMessage(string.Format(Message.RequiredField, "Số lượng hành khách tối thiểu"))
                .GreaterThanOrEqualTo(1).WithMessage("MinParticipants must be at least 1")
                .LessThanOrEqualTo(x => x.TourRequest.MaxParticipants ?? int.MaxValue)
                .WithMessage("MinParticipants must not exceed MaxParticipants");

            RuleFor(x => x.TourRequest.BasePriceAdult)
                .NotNull().WithMessage(string.Format(Message.RequiredField, "Giá vé cơ bản người lớn"))
                .GreaterThan(0).WithMessage("BasePriceAdult must be greater than 0");

            RuleFor(x => x.TourRequest.BasePriceChild)
                .NotNull().WithMessage(string.Format(Message.RequiredField, "Giá vé cơ bản trẻ em"))
                .GreaterThanOrEqualTo(0).WithMessage("BasePriceChild must be greater than or equal to 0");

        }
    }
}
