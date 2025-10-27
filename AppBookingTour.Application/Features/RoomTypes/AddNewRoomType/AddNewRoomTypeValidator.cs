using AppBookingTour.Domain.Constants;
using FluentValidation;

namespace AppBookingTour.Application.Features.RoomTypes.AddNewRoomType
{
    public class AddNewRoomTypeValidator : AbstractValidator<AddNewRoomTypeCommand>
    {
        public AddNewRoomTypeValidator()
        {
            RuleFor(x => x.RoomType)
                .SetValidator(new RoomTypeDtoValidator());
        }
    }
    public class RoomTypeDtoValidator : AbstractValidator<AddNewRoomTypeDTO>
    {
        public RoomTypeDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(string.Format(Message.RequiredField, "Tên loại phòng"));
            RuleFor(x => x.AccommodationId).NotEmpty().WithMessage(string.Format(Message.RequiredField, "Chỗ ở"));
        }
    }
}
