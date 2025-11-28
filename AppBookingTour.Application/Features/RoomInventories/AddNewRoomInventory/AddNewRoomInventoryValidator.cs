using AppBookingTour.Domain.Constants;
using FluentValidation;

namespace AppBookingTour.Application.Features.RoomInventories.AddNewRoomInventory
{
    public class AddNewRoomInventoryValidator : AbstractValidator<AddNewRoomInventoryCommand>
    {
        public AddNewRoomInventoryValidator()
        {
            RuleFor(x => x.RoomInventory)
                .NotNull()
                .WithMessage("Dữ liệu yêu cầu không hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.RoomInventory!.RoomTypeId)
                        .GreaterThan(0)
                        .WithMessage(string.Format(Message.RequiredField, "Loại phòng"));

                    RuleFor(x => x.RoomInventory!.Date)
                        .NotEmpty()
                        .WithMessage(string.Format(Message.RequiredField, "Ngày tồn kho"));

                    RuleFor(x => x.RoomInventory!.BasePrice)
                        .GreaterThanOrEqualTo(0)
                        .WithMessage("Giá phải lớn hơn hoặc bằng 0.");

                    RuleFor(x => x.RoomInventory!.BookedRooms)
                        .GreaterThanOrEqualTo(0)
                        .WithMessage("Số slot phải lớn hơn hoặc bằng 0.");
                });
        }
    }
}


