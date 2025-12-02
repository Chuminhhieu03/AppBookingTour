using AppBookingTour.Domain.Enums;
using MediatR;

namespace AppBookingTour.Application.Features.Statistics.ExportItemStatisticByRevenue;

public record ExportItemStatisticByRevenueQuery(
    DateOnly StartDate,
    DateOnly EndDate,
    ItemType ItemType,
    bool? IsDesc
) : IRequest<ExportFileDTO>;