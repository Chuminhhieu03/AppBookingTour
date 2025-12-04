using FluentValidation;

namespace AppBookingTour.Application.Features.Reviews.CreateReview;

public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(x => x.Request.UserId)
            .GreaterThan(0).WithMessage("UserId phải lớn hơn 0");

        RuleFor(x => x.Request.BookingId)
            .GreaterThan(0).WithMessage("BookingId phải lớn hơn 0");

        RuleFor(x => x.Request.Rating)
            .InclusiveBetween(1, 10).WithMessage("Rating phải từ 1 đến 10");

        RuleFor(x => x.Request.Comment)
            .MaximumLength(1000).WithMessage("Comment không được vượt quá 1000 ký tự")
            .When(x => !string.IsNullOrEmpty(x.Request.Comment));
    }
}
