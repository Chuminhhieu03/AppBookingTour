using MediatR;

namespace AppBookingTour.Application.Features.Statistics.OverviewStatistic;

public record OverviewStatisticQuery(int year, int month) : IRequest<OverviewStatisticDTO>;
