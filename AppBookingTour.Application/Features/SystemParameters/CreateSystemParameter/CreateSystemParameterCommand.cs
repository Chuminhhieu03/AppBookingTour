using MediatR;

namespace AppBookingTour.Application.Features.SystemParameters.CreateSystemParameter
{
    public record CreateSystemParameterCommand(SystemParameterRequestDTO RequestDto) : IRequest<CreateSystemParameterResponse>;
}