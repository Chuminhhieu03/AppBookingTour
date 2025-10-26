using MediatR;

namespace AppBookingTour.Application.Features.SystemParameters.DeleteSystemParameter
{
    public record DeleteSystemParameterCommand(int Id) : IRequest<DeleteSystemParameterResponse>;
}