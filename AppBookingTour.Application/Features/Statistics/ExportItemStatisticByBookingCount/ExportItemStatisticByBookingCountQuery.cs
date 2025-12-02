using MediatR;
using AppBookingTour.Domain.Enums;
using AppBookingTour.Application.Features.Statistics.ExportItemStatisticByRevenue;

namespace AppBookingTour.Application.Features.Statistics.ExportItemStatisticByBookingCount;

public record ExportItemStatisticByBookingCountQuery(
    DateOnly StartDate,
    DateOnly EndDate,
    ItemType ItemType,
    bool? IsDesc
) : IRequest<ExportFileDTO>;