using AppBookingTour.Domain.Constants;
using FluentValidation;

namespace AppBookingTour.Application.Features.TourItineraries.UpdateTourItinerary;

public class UpdateTourItineraryCommandValidator : AbstractValidator<UpdateTourItineraryCommand>
{
    public UpdateTourItineraryCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.TourItineraryRequest.TourId)
            .NotNull().WithMessage(string.Format(Message.RequiredField, "Tour Id"))
            .GreaterThan(0).WithMessage("TourId must be greater than 0");

        RuleFor(x => x.TourItineraryRequest.DayNumber)
            .NotNull().WithMessage(string.Format(Message.RequiredField, "Ngày"))
            .GreaterThan(0).WithMessage("DayNumber must be greater than 0");

        RuleFor(x => x.TourItineraryRequest.Title)
            .NotEmpty().WithMessage(string.Format(Message.RequiredField, "Tiêu đề"))
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

        RuleFor(x => x.TourItineraryRequest.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters");

        RuleFor(x => x.TourItineraryRequest.Activity)
            .MaximumLength(1000).WithMessage("Activity must not exceed 1000 characters");

        RuleFor(x => x.TourItineraryRequest.Note)
            .MaximumLength(500).WithMessage("Note must not exceed 500 characters");
    }
}
