using MediatR;

namespace AppBookingTour.Application.Features.TourDepartures.GetListTourDepartureForGuide;

public record GetListTourDepartureForGuideQuery(int GuideId) : IRequest<List<TourDepartureItemForGuide>>;
