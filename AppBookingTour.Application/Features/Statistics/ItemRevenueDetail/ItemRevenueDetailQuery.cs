using AppBookingTour.Domain.Enums;
using MediatR;

namespace AppBookingTour.Application.Features.Statistics.ItemRevenueDetail;
public record ItemRevenueDetailQuery(DateOnly StartDate, DateOnly EndDate, ItemType ItemType, int ItemId) : IRequest<ItemRevenueDetailResponse>;