using MediatR;

namespace AppBookingTour.Application.Features.SystemParameters.GetSystemParameterById
{
    public record GetSystemParameterByIdQuery(int Id) : IRequest<GetSystemParameterByIdResponse>;
}