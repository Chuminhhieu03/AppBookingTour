using MediatR;
using AppBookingTour.Application.Features.SystemParameters.CreateSystemParameter;

namespace AppBookingTour.Application.Features.SystemParameters.UpdateSystemParameter
{
    public record UpdateSystemParameterCommand(int Id, SystemParameterRequestDTO RequestDto) : IRequest<UpdateSystemParameterResponse>;
}