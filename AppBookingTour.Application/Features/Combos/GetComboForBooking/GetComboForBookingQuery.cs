using MediatR;

namespace AppBookingTour.Application.Features.Combos.GetComboForBooking;

/// <summary>
/// Query to get combo with specific schedule for booking
/// </summary>
/// <param name="ScheduleId">The combo schedule ID that user selected</param>
public record GetComboForBookingQuery(int ScheduleId) : IRequest<ComboForBookingDTO?>;
