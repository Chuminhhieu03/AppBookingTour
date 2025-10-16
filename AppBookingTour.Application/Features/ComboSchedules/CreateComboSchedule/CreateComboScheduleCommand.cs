using MediatR;

namespace AppBookingTour.Application.Features.ComboSchedules.CreateComboSchedule;

public record CreateComboScheduleCommand(ComboScheduleRequestDTO ComboScheduleRequest) : IRequest<CreateComboScheduleResponse>;