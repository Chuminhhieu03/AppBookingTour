using AppBookingTour.Application.Features.TourDepartures.CreateTourDeparture;
using AppBookingTour.Application.Features.TourItineraries.CreateTourItinerary;
using AppBookingTour.Domain.Constants;
using FluentValidation;

namespace AppBookingTour.Application.Features.Tours.CreateTour
{
    public class CreateTourCommandValidator : AbstractValidator<CreateTourCommand>
    {
        public CreateTourCommandValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.TourRequest.Code)
                .NotEmpty().WithMessage(string.Format(Message.RequiredField, "Mã tour"))
                .MaximumLength(50).WithMessage("Mã tour không được vượt quá 50 ký tự");

            RuleFor(x => x.TourRequest.Name)
                .NotEmpty().WithMessage(string.Format(Message.RequiredField, "Tên tour"))
                .MaximumLength(200).WithMessage("Tên tour không được vượt quá 200 ký tự");

            RuleFor(x => x.TourRequest.TypeId)
                .NotNull().WithMessage(string.Format(Message.RequiredField, "Loại tour"))
                .GreaterThan(0).WithMessage("Loại tour không hợp lệ");

            RuleFor(x => x.TourRequest.CategoryId)
                .NotNull().WithMessage(string.Format(Message.RequiredField, "Danh mục tour"))
                .GreaterThan(0).WithMessage("Danh mục tour không hợp lệ");

            RuleFor(x => x.TourRequest.DepartureCityId)
                .NotNull().WithMessage(string.Format(Message.RequiredField, "Thành phố khởi hành"))
                .GreaterThan(0).WithMessage("Thành phố khởi hành không hợp lệ");

            RuleFor(x => x.TourRequest.DestinationCityId)
                .NotNull().WithMessage(string.Format(Message.RequiredField, "Thành phố tham quan"))
                .GreaterThan(0).WithMessage("Thành phố tham quan không hợp lệ");

            RuleFor(x => x)
                .Must(x => x.TourRequest.DepartureCityId != x.TourRequest.DestinationCityId)
                .WithMessage("Thành phố khởi hành không được trùng với thành phố tham quan.");

            RuleFor(x => x.TourRequest.DurationDays)
                .NotNull().WithMessage(string.Format(Message.RequiredField, "Số ngày lưu trú"))
                .GreaterThan(0).WithMessage("Số ngày lưu trú phải lớn hơn 0");

            RuleFor(x => x.TourRequest.DurationNights)
                .NotNull().WithMessage(string.Format(Message.RequiredField, "Số đêm lưu trú"))
                .GreaterThanOrEqualTo(0).WithMessage("Số đêm lưu trú phải lớn hơn hoặc bằng 0");

            RuleFor(x => x.TourRequest.MaxParticipants)
                .NotNull().WithMessage(string.Format(Message.RequiredField, "Số lượng hành khách tối đa"))
                .GreaterThan(0).WithMessage("Số lượng hành khách tối đa phải lớn hơn 0");

            RuleFor(x => x.TourRequest.MinParticipants)
                .NotNull().WithMessage(string.Format(Message.RequiredField, "Số lượng hành khách tối thiểu"))
                .GreaterThanOrEqualTo(1).WithMessage("Số lượng hành khách tối thiểu phải từ 1 trở lên")
                .LessThanOrEqualTo(x => x.TourRequest.MaxParticipants ?? int.MaxValue)
                .WithMessage("Số lượng hành khách tối thiểu không được vượt quá số lượng hành khách tối đa");

            RuleFor(x => x.TourRequest.BasePriceAdult)
                .NotNull().WithMessage(string.Format(Message.RequiredField, "Giá vé cơ bản người lớn"))
                .GreaterThan(0).WithMessage("Giá vé cơ bản người lớn phải lớn hơn 0");

            RuleFor(x => x.TourRequest.BasePriceChild)
                .NotNull().WithMessage(string.Format(Message.RequiredField, "Giá vé cơ bản trẻ em"))
                .GreaterThanOrEqualTo(0).WithMessage("Giá vé cơ bản trẻ em phải lớn hơn hoặc bằng 0");

            RuleFor(x => x.TourRequest.Description)
                .MaximumLength(3000).WithMessage("Mô tả không được vượt quá 3000 ký tự");

            RuleForEach(x => x.TourRequest.Itineraries)
                .SetValidator(new TourItineraryRequestValidator());

            RuleForEach(x => x.TourRequest.Departures)
                .SetValidator(new TourDepartureRequestValidator());
        }
    }
}