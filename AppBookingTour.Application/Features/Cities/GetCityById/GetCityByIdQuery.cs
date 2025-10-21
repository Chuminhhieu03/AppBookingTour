using MediatR;

namespace AppBookingTour.Application.Features.Cities.GetCityById;

public record GetCityByIdQuery(int CityId) : IRequest<GetCityByIdResponse>;