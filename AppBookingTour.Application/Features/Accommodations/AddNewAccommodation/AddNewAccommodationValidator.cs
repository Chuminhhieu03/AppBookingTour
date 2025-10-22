using AppBookingTour.Domain.Constants;
using FluentValidation;

namespace AppBookingTour.Application.Features.Accommodations.AddNewAccommodation
{
    public class AddNewAccommodationValidator : AbstractValidator<AddNewAccommodationCommand>
    {
        public AddNewAccommodationValidator()
        {
            RuleFor(x => x.Accommodation)
                .SetValidator(new AccommodationDtoValidator());
        }
    }
    public class AccommodationDtoValidator : AbstractValidator<AddNewAccommodationDTO>
    {
        public AccommodationDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(string.Format(Message.RequiredField, "Tên chỗ ở"));
            RuleFor(x => x.CityId).NotEmpty().WithMessage(string.Format(Message.RequiredField, "Thành phố"));
            RuleFor(x => x.StarRating).GreaterThanOrEqualTo(0).WithMessage("Số sao phải >= 0");
        }
    }
}
