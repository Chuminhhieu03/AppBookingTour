using AppBookingTour.Domain.Constants;
using FluentValidation;

namespace AppBookingTour.Application.Features.RoomInventories.UpdateRoomInventory
{
    public class UpdateRoomInventoryValidator : AbstractValidator<UpdateRoomInventoryCommand>
    {
        public UpdateRoomInventoryValidator()
        {
            RuleFor(x => x.RoomInventory)
                .SetValidator(new RoomInventoryDtoValidator());
        }
    }
    public class RoomInventoryDtoValidator : AbstractValidator<UpdateRoomInventoryDTO>
    {
        public RoomInventoryDtoValidator()
        {
            RuleFor(x => x.RoomTypeId).NotEmpty().WithMessage(string.Format(Message.RequiredField, "Loại phòng"));
            RuleFor(x => x.Date).NotEmpty().WithMessage(string.Format(Message.RequiredField, "Ngày tồn kho"));
        }
    }
}
