using AppBookingTour.Domain.Enums;
using MediatR;

namespace AppBookingTour.Application.Features.Statistics.ItemStatisticByBookingCount;

public record ItemStatisticByBookingCountQuery(DateOnly StartDate, DateOnly EndDate, ItemType ItemType, int? PageIndex, int? PageSize, bool? IsDesc) : IRequest<ItemStatisticByBookingCountResponse>;