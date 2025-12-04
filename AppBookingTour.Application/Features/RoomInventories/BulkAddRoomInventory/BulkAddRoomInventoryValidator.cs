using AppBookingTour.Domain.Constants;
using FluentValidation;

namespace AppBookingTour.Application.Features.RoomInventories.BulkAddRoomInventory
{
    public class BulkAddRoomInventoryValidator : AbstractValidator<BulkAddRoomInventoryCommand>
    {
        public BulkAddRoomInventoryValidator()
        {
            RuleFor(x => x.Request)
                .NotNull()
                .WithMessage("Dữ liệu yêu cầu không hợp lệ.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.Request!.RoomTypeId)
                        .GreaterThan(0)
                        .WithMessage(string.Format(Message.RequiredField, "Loại phòng"));

                    RuleFor(x => x.Request!.BookedRooms)
                        .GreaterThanOrEqualTo(0)
                        .WithMessage("Số slot phải lớn hơn hoặc bằng 0.");

                    RuleFor(x => x.Request!.BasePrice)
                        .GreaterThanOrEqualTo(0)
                        .WithMessage("Giá phải lớn hơn hoặc bằng 0.");

                    RuleFor(x => x.Request!.FromDate)
                        .NotEmpty()
                        .WithMessage(string.Format(Message.RequiredField, "Ngày bắt đầu"));

                    RuleFor(x => x.Request!.ToDate)
                        .NotEmpty()
                        .WithMessage(string.Format(Message.RequiredField, "Ngày kết thúc"));

                    RuleFor(x => x.Request!)
                        .Must(r => r.ToDate.Date >= r.FromDate.Date)
                        .WithMessage("Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu.");
                });
        }
    }
}


