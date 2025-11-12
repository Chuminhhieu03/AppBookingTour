using MediatR;

namespace AppBookingTour.Application.Features.Statistics.OverviewStatistic;

public record OverviewStatisticQuery() : IRequest<OverviewStatisticDTO>;
