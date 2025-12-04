using AppBookingTour.Domain.Enums;
using MediatR;

namespace AppBookingTour.Application.Features.Statistics.ItemStatisticByRevenue;

public record ItemStatisticByRevenueQuery(DateOnly StartDate, DateOnly EndDate, ItemType ItemType, int? PageIndex, int? PageSize, bool? IsDesc) : IRequest<ItemStatisticByRevenueResponse>;