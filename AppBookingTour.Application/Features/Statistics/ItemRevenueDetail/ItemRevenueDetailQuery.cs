using MediatR;

namespace AppBookingTour.Application.Features.Statistics.ItemRevenueDetail;
//TODO: Change ItemType to Enum
public record ItemRevenueDetailQuery(DateOnly StartDate, DateOnly EndDate, int ItemType, int ItemId) : IRequest<ItemRevenueDetailResponse>;