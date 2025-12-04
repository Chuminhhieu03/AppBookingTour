using FluentValidation;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Application.Features.Combos.CreateCombo;

public class CreateComboCommandValidator : AbstractValidator<CreateComboCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateComboCommandValidator(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

        RuleFor(x => x.ComboRequest.Code)
            .NotEmpty().WithMessage("Mã combo không được để trống")
            .MaximumLength(50).WithMessage("Mã combo không được vượt quá 50 ký tự")
            .Matches(@"^[A-Z0-9-]+$").WithMessage("Mã combo chỉ được chứa chữ in hoa, số và dấu gạch ngang")
            .MustAsync(BeUniqueCode).WithMessage("Mã combo đã tồn tại");

        RuleFor(x => x.ComboRequest.Name)
            .NotEmpty().WithMessage("Tên combo không được để trống")
            .MaximumLength(200).WithMessage("Tên combo không được vượt quá 200 ký tự");

        RuleFor(x => x.ComboRequest.FromCityId)
            .NotNull().WithMessage("Thành phố xuất phát không được để trống")
            .GreaterThan(0).WithMessage("Thành phố xuất phát không hợp lệ")
            .MustAsync(CityExists).WithMessage("Thành phố xuất phát không tồn tại");

        RuleFor(x => x.ComboRequest.ToCityId)
            .NotNull().WithMessage("Thành phố đến không được để trống")
            .GreaterThan(0).WithMessage("Thành phố đến không hợp lệ")
            .MustAsync(CityExists).WithMessage("Thành phố đến không tồn tại");

        RuleFor(x => x.ComboRequest)
            .Must(x => x.FromCityId != x.ToCityId)
            .WithMessage("Thành phố xuất phát và đến không được giống nhau")
            .When(x => x.ComboRequest.FromCityId.HasValue && x.ComboRequest.ToCityId.HasValue);

        RuleFor(x => x.ComboRequest.Vehicle)
            .NotNull().WithMessage("Phương tiện không được để trống")
            .Must(v => v == (int)Vehicle.Car || v == (int)Vehicle.Plane)
            .WithMessage("Phương tiện phải là Car (1) hoặc Plane (2)");

        RuleFor(x => x.ComboRequest.DurationDays)
            .NotNull().WithMessage("Số ngày du lịch không được để trống")
            .GreaterThan(0).WithMessage("Số ngày du lịch phải lớn hơn 0")
            .LessThanOrEqualTo(30).WithMessage("Số ngày du lịch không được vượt quá 30 ngày");

        RuleFor(x => x.ComboRequest.BasePriceAdult)
            .NotNull().WithMessage("Giá người lớn không được để trống")
            .GreaterThan(0).WithMessage("Giá người lớn phải lớn hơn 0")
            .LessThan(100000000).WithMessage("Giá người lớn không hợp lý");

        RuleFor(x => x.ComboRequest.BasePriceChildren)
            .NotNull().WithMessage("Giá trẻ em không được để trống")
            .GreaterThanOrEqualTo(0).WithMessage("Giá trẻ em không được âm")
            .LessThan(100000000).WithMessage("Giá trẻ em không hợp lý");

        RuleFor(x => x.ComboRequest)
            .Must(x => x.BasePriceChildren <= x.BasePriceAdult)
            .WithMessage("Giá trẻ em không được cao hơn giá người lớn")
            .When(x => x.ComboRequest.BasePriceChildren.HasValue && x.ComboRequest.BasePriceAdult.HasValue);

        RuleFor(x => x.ComboRequest.ShortDescription)
            .MaximumLength(500).WithMessage("Mô tả ngắn không được vượt quá 500 ký tự")
            .When(x => !string.IsNullOrEmpty(x.ComboRequest.ShortDescription));

        RuleFor(x => x.ComboRequest.Description)
            .MaximumLength(10000).WithMessage("Mô tả chi tiết không được vượt quá 10,000 ký tự")
            .When(x => !string.IsNullOrEmpty(x.ComboRequest.Description));

        RuleFor(x => x.ComboRequest.AdditionalInfo)
            .MaximumLength(4000).WithMessage("Thông tin thêm không được vượt quá 4000 ký tự")
            .When(x => !string.IsNullOrEmpty(x.ComboRequest.AdditionalInfo));

        RuleFor(x => x.ComboRequest.ImportantInfo)
            .MaximumLength(10000).WithMessage("Thông tin quan trọng không được vượt quá 10000 ký tự")
            .When(x => !string.IsNullOrEmpty(x.ComboRequest.ImportantInfo));

        RuleFor(x => x.ComboRequest.Schedules)
            .NotEmpty().WithMessage("Combo phải có ít nhất một lịch khởi hành")
            .Must(s => s != null && s.Count > 0).WithMessage("Combo phải có ít nhất một lịch khởi hành");

        RuleForEach(x => x.ComboRequest.Schedules)
            .ChildRules(schedule =>
            {
                schedule.RuleFor(s => s.DepartureDate)
                    .NotNull().WithMessage("Ngày khởi hành không được để trống")
                    .GreaterThan(DateTime.UtcNow).WithMessage("Ngày khởi hành phải sau ngày hiện tại");

                schedule.RuleFor(s => s.ReturnDate)
                    .NotNull().WithMessage("Ngày về không được để trống")
                    .GreaterThan(s => s.DepartureDate ?? DateTime.MinValue)
                    .WithMessage("Ngày về phải sau ngày khởi hành");

                schedule.RuleFor(s => s.AvailableSlots)
                    .NotNull().WithMessage("Số chỗ trống không được để trống")
                    .GreaterThan(0).WithMessage("Số chỗ trống phải lớn hơn 0")
                    .LessThanOrEqualTo(500).WithMessage("Số chỗ trống không được vượt quá 500");

                schedule.RuleFor(s => s.BasePriceAdult)
                    .NotNull().WithMessage("Giá người lớn cho schedule không được để trống")
                    .GreaterThan(0).WithMessage("Giá người lớn phải lớn hơn 0");

                schedule.RuleFor(s => s.BasePriceChildren)
                    .NotNull().WithMessage("Giá trẻ em cho schedule không được để trống")
                    .GreaterThanOrEqualTo(0).WithMessage("Giá trẻ em không được âm");
            })
            .When(x => x.ComboRequest.Schedules != null);
    }

    private async Task<bool> BeUniqueCode(string? code, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(code))
            return true;

        var exists = await _unitOfWork.Repository<Combo>()
            .ExistsAsync(c => c.Code == code, cancellationToken);
        return !exists;
    }

    private async Task<bool> CityExists(int? cityId, CancellationToken cancellationToken)
    {
        if (!cityId.HasValue || cityId.Value <= 0)
            return false;

        return await _unitOfWork.Repository<City>()
            .ExistsAsync(c => c.Id == cityId.Value, cancellationToken);
    }
}