using AppBookingTour.Domain.Enums;
using MediatR;

namespace AppBookingTour.Application.Features.Statistics.ItemBookingCountDetail;

public record ItemBookingCountDetailQuery(DateOnly StartDate, DateOnly EndDate, ItemType ItemType, int ItemId) : IRequest<ItemBookingCountDetailResponse>;