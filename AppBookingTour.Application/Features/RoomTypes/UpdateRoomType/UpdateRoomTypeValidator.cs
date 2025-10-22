using AppBookingTour.Domain.Constants;
using FluentValidation;

namespace AppBookingTour.Application.Features.RoomTypes.UpdateRoomType
{
    public class UpdateRoomTypeValidator : AbstractValidator<UpdateRoomTypeCommand>
    {
        public UpdateRoomTypeValidator()
        {
            RuleFor(x => x.RoomType)
                .SetValidator(new RoomTypeDtoValidator());
        }
    }
    public class RoomTypeDtoValidator : AbstractValidator<UpdateRoomTypeDTO>
    {
        public RoomTypeDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(string.Format(Message.RequiredField, "Tên loại phòng"));
            RuleFor(x => x.AccommodationId).NotEmpty().WithMessage(string.Format(Message.RequiredField, "Chỗ ở"));
            RuleFor(x => x.Capacity).GreaterThanOrEqualTo(1).WithMessage("Sức chứa phải >= 1");
        }
    }
}
