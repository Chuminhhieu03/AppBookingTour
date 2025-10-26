using MediatR;

using AppBookingTour.Application.Features.ComboSchedules.CreateComboSchedule;

namespace AppBookingTour.Application.Features.ComboSchedules.UpdateComboSchedule;

public record UpdateComboScheduleCommand(int ComboScheduleId, ComboScheduleRequestDTO ComboScheduleRequest) : IRequest<UpdateComboScheduleResponse>;