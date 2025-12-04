using FluentValidation;

namespace AppBookingTour.Application.Features.RoomInventories.BulkDeleteRoomInventory
{
    public class BulkDeleteRoomInventoryValidator : AbstractValidator<BulkDeleteRoomInventoryCommand>
    {
        public BulkDeleteRoomInventoryValidator()
        {
            RuleFor(x => x.Request)
                .NotNull()
                .WithMessage("Dữ liệu yêu cầu không hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Request!.Ids)
                        .NotEmpty()
                        .WithMessage("Danh sách id cần xóa không được để trống.");
                });
        }
    }
}


