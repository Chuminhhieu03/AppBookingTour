using AppBookingTour.Domain.Constants;
using FluentValidation;

namespace AppBookingTour.Application.Features.TourDepartures.CreateTourDeparture;

public class TourDepartureRequestValidator : AbstractValidator<TourDepartureRequestDTO>
{
    const int VnOffset = 7;
    public TourDepartureRequestValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.DepartureDate)
            .NotNull().WithMessage(string.Format(Message.RequiredField, "Ngày khởi hành"))
            .Must(BeAValidDate).WithMessage("Ngày khởi hành không hợp lệ")
            .Must(BeFutureDate).WithMessage("Ngày khởi hành phải là ngày trong tương lai");

        RuleFor(x => x.ReturnDate)
            .NotNull().WithMessage(string.Format(Message.RequiredField, "Ngày trở về"))
            .Must(BeAValidDate).WithMessage("Ngày trở về không hợp lệ")
            .GreaterThan(x => x.DepartureDate)
            .WithMessage("Ngày trở về phải sau ngày khởi hành");

        RuleFor(x => x.AvailableSlots)
            .NotNull().WithMessage(string.Format(Message.RequiredField, "Số lượng còn lại"))
            .GreaterThan(0).WithMessage("Số chỗ còn lại phải lớn hơn 0");

        RuleFor(x => x.PriceAdult)
            .NotNull().WithMessage(string.Format(Message.RequiredField, "Giá vé người lớn"))
            .GreaterThan(0).WithMessage("Giá vé người lớn phải lớn hơn 0");

        RuleFor(x => x.PriceChildren)
            .NotNull().WithMessage(string.Format(Message.RequiredField, "Giá vé trẻ em"))
            .GreaterThanOrEqualTo(0).WithMessage("Giá vé trẻ em phải lớn hơn hoặc bằng 0");

        RuleFor(x => x.SingleRoomSurcharge)
            .NotNull().WithMessage(string.Format(Message.RequiredField, "Phụ thu phòng đơn"))
            .GreaterThanOrEqualTo(0).WithMessage("Phụ thu phòng đơn phải lớn hơn hoặc bằng 0");

        RuleFor(x => x.Status)
            .NotNull().WithMessage(string.Format(Message.RequiredField, "Trạng thái"))
            .InclusiveBetween(1, 3).WithMessage("Trạng thái không hợp lệ (1 = Sẵn sàng, 2 = Đầy, 3 = Hủy)");

        RuleFor(x => x.GuideId)
            .GreaterThan(0).When(x => x.GuideId.HasValue)
            .WithMessage("Mã hướng dẫn viên phải lớn hơn 0 nếu được cung cấp");
    }

    private bool BeAValidDate(DateTime? date)
    {
        return date.HasValue && date.Value != default;
    }

    private bool BeFutureDate(DateTime? date)
    {
        if (!date.HasValue) return false;
        return date.Value.AddHours(VnOffset).Date > DateTime.UtcNow.AddHours(VnOffset).Date;
    }
}