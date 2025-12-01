using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Combos.GetComboForBooking;

/// <summary>
/// Handler for GetComboForBooking - Get combo with selected schedule for booking
/// </summary>
public class GetComboForBookingQueryHandler : IRequestHandler<GetComboForBookingQuery, ComboForBookingDTO?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetComboForBookingQueryHandler> _logger;

    public GetComboForBookingQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetComboForBookingQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ComboForBookingDTO?> Handle(GetComboForBookingQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting combo for booking with schedule ID: {ScheduleId}", request.ScheduleId);

        // Get the selected schedule
        var schedule = await _unitOfWork.Repository<ComboSchedule>()
            .GetByIdAsync(request.ScheduleId, cancellationToken);

        if (schedule == null)
        {
            _logger.LogWarning("ComboSchedule not found: {ScheduleId}", request.ScheduleId);
            return null;
        }

        // Get combo with navigation properties
        var combo = await _unitOfWork.Combos.GetComboWithDetailsAsync(schedule.ComboId, cancellationToken);

        if (combo == null)
        {
            _logger.LogWarning("Combo not found: {ComboId}", schedule.ComboId);
            return null;
        }

        if (!combo.IsActive)
        {
            _logger.LogWarning("Combo is inactive: {ComboId}", combo.Id);
            return null;
        }

        _logger.LogInformation("Successfully retrieved combo {ComboId} with schedule {ScheduleId} for booking",
            combo.Id, schedule.Id);

        return new ComboForBookingDTO
        {
            Combo = new ComboInfoDTO
            {
                Id = combo.Id,
                Code = combo.Code,
                Name = combo.Name,
                ShortDescription = combo.ShortDescription,
                Description = combo.Description,
                ComboImageCoverUrl = combo.ComboImageCoverUrl,
                DurationDays = combo.DurationDays,
                Vehicle = combo.Vehicle.ToString(),
                Rating = combo.Rating,
                TotalBookings = combo.TotalBookings,
                FromCityName = combo.FromCity.Name,
                ToCityName = combo.ToCity.Name
            },
            Schedule = new ComboScheduleInfoDTO
            {
                Id = schedule.Id,
                DepartureDate = schedule.DepartureDate,
                ReturnDate = schedule.ReturnDate,
                BasePriceAdult = schedule.BasePriceAdult,
                BasePriceChildren = schedule.BasePriceChildren,
                SingleRoomSupplement = schedule.SingleRoomSupplement,
                AvailableSlots = schedule.AvailableSlots,
                BookedSlots = schedule.BookedSlots,
                Status = schedule.Status.ToString()
            }
        };
    }
}
