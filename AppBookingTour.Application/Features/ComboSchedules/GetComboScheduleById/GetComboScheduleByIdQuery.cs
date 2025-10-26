using MediatR;

namespace AppBookingTour.Application.Features.ComboSchedules.GetComboScheduleById;

public record GetComboScheduleByIdQuery(int ComboScheduleId) : IRequest<GetComboScheduleByIdResponse>;