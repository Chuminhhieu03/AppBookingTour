using MediatR;

namespace AppBookingTour.Application.Features.Profiles.GetListGuide;

public record GetListGuideQuery() : IRequest<List<GuideItemDTO>>;