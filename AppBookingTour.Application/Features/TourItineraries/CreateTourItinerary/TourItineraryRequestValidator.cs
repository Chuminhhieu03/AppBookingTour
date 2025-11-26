using AppBookingTour.Domain.Constants;
using FluentValidation;

namespace AppBookingTour.Application.Features.TourItineraries.CreateTourItinerary;

public class TourItineraryRequestValidator : AbstractValidator<TourItineraryRequestDTO>
{
    public TourItineraryRequestValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.DayNumber)
            .NotNull().WithMessage(string.Format(Message.RequiredField, "Ngày"))
            .GreaterThan(0).WithMessage("Ngày phải lớn hơn 0");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage(string.Format(Message.RequiredField, "Tiêu đề"))
            .MaximumLength(200).WithMessage("Tiêu đề không được vượt quá 200 ký tự");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Mô tả không được vượt quá 500 ký tự");

        RuleFor(x => x.Activity)
            .MaximumLength(1000).WithMessage("Hoạt động không được vượt quá 1000 ký tự");

        RuleFor(x => x.Note)
            .MaximumLength(500).WithMessage("Ghi chú không được vượt quá 500 ký tự");
    }
}