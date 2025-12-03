using AppBookingTour.Domain.Constants;
using FluentValidation;

namespace AppBookingTour.Application.Features.Accommodations.UpdateAccommodation
{
    public class UpdateAccommodationValidator : AbstractValidator<UpdateAccommodationCommand>
    {
        public UpdateAccommodationValidator()
        {
            RuleFor(x => x.Accommodation)
                .SetValidator(new AccommodationDtoValidator());
        }
    }
    public class AccommodationDtoValidator : AbstractValidator<UpdateAccommodationDTO>
    {
        public AccommodationDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(string.Format(Message.RequiredField, "Tên chỗ ở"));
            RuleFor(x => x.CityId).NotEmpty().WithMessage(string.Format(Message.RequiredField, "Thành phố"));
            RuleFor(x => x.StarRating).GreaterThanOrEqualTo(0).WithMessage("Số sao phải >= 0");
            
            RuleFor(x => x.Coordinates)
                .Must(BeValidCoordinates)
                .When(x => !string.IsNullOrWhiteSpace(x.Coordinates))
                .WithMessage("Tọa độ không hợp lệ. Định dạng phải là: lat, lon (ví dụ: 10.123, 106.456). Latitude phải từ -90 đến 90, Longitude phải từ -180 đến 180.");
        }

        private bool BeValidCoordinates(string? coordinates)
        {
            if (string.IsNullOrWhiteSpace(coordinates))
                return true;

            var parts = coordinates.Split(',');
            if (parts.Length != 2)
                return false;

            if (!double.TryParse(parts[0].Trim(), out double lat) || 
                !double.TryParse(parts[1].Trim(), out double lon))
                return false;

            // Validate latitude: -90 to 90
            if (lat < -90 || lat > 90)
                return false;

            // Validate longitude: -180 to 180
            if (lon < -180 || lon > 180)
                return false;

            return true;
        }
    }
}
