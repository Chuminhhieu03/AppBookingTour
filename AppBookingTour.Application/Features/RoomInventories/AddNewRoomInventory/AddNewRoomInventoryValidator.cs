using AppBookingTour.Domain.Constants;
using FluentValidation;

namespace AppBookingTour.Application.Features.RoomInventories.AddNewRoomInventory
{
    public class AddNewRoomInventoryValidator : AbstractValidator<AddNewRoomInventoryCommand>
    {
        public AddNewRoomInventoryValidator()
        {
            RuleFor(x => x.RoomInventory)
                .SetValidator(new RoomInventoryDtoValidator());
        }
    }
    public class RoomInventoryDtoValidator : AbstractValidator<AddNewRoomInventoryDTO>
    {
        public RoomInventoryDtoValidator()
        {
            RuleFor(x => x.RoomTypeId).NotEmpty().WithMessage(string.Format(Message.RequiredField, "Loại phòng"));
            RuleFor(x => x.Date).NotEmpty().WithMessage(string.Format(Message.RequiredField, "Ngày tồn kho"));
            RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0).WithMessage("Số lượng phải >= 0");
        }
    }
}
