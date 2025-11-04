using MediatR;

namespace AppBookingTour.Application.Features.Statistics.ItemStatisticByRevenue;
//TODO: Change ItemType to Enum
public record ItemStatisticByRevenueQuery(DateOnly StartDate, DateOnly EndDate, int ItemType) : IRequest<ItemStatisticByRevenueResponse>;