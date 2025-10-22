using MediatR;

namespace AppBookingTour.Application.Features.Cities.GetListCity;

public record GetListCityQuery() : IRequest<GetListCityResponse>;