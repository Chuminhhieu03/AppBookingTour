using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.Enums;
using MediatR;

namespace AppBookingTour.Application.Features.SystemParameters.GetSystemParameterByFeatureCode
{
    public record GetSystemParameterByFeatureCodeQuery(FeatureCode FeatureCode) : IRequest<List<SystemParameter>>;
}