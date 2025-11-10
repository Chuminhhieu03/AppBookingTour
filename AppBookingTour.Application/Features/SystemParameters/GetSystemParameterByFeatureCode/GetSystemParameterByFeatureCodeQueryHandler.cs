using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.SystemParameters.GetSystemParameterByFeatureCode
{
    public class GetSystemParameterByFeatureCodeQueryHandler : IRequestHandler<GetSystemParameterByFeatureCodeQuery, List<SystemParameter>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSystemParameterByFeatureCodeQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SystemParameter>> Handle(GetSystemParameterByFeatureCodeQuery request, CancellationToken cancellationToken)
        {
            var listSystemParameter = await _unitOfWork.SystemParameters.GetListSystemParameterByFeatureCode(request.FeatureCode);
            return listSystemParameter;
        }
    }
}