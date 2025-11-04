using MediatR;

namespace AppBookingTour.Application.Features.Statistics.ItemBookingCountDetail;

public record ItemBookingCountDetailQuery(DateOnly StartDate, DateOnly EndDate, int ItemType, int ItemId) : IRequest<ItemBookingCountDetailResponse>;