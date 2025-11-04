using MediatR;

namespace AppBookingTour.Application.Features.Statistics.ItemStatisticByBookingCount;

public record ItemStatisticByBookingCountQuery(DateOnly StartDate, DateOnly EndDate, int ItemType) : IRequest<ItemStatisticByBookingCountResponse>;