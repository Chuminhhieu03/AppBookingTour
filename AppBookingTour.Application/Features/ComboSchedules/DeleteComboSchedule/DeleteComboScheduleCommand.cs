using MediatR;

namespace AppBookingTour.Application.Features.ComboSchedules.DeleteComboSchedule;

public record DeleteComboScheduleCommand(int ComboScheduleId) : IRequest<DeleteComboScheduleResponse>;