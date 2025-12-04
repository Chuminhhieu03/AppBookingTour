using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.SystemParameters.GetSystemParameterById
{
    public sealed class GetSystemParameterByIdQueryHandler : IRequestHandler<GetSystemParameterByIdQuery, GetSystemParameterByIdResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetSystemParameterByIdQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetSystemParameterByIdQueryHandler(
            IUnitOfWork unitOfWork,
            ILogger<GetSystemParameterByIdQueryHandler> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<GetSystemParameterByIdResponse> Handle(GetSystemParameterByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting system parameter details for ID: {Id}", request.Id);
            try
            {
                var systemParameter = await _unitOfWork.Repository<SystemParameter>().GetByIdAsync(request.Id, cancellationToken);

                if (systemParameter == null)
                {
                    _logger.LogWarning("System parameter not found with ID: {Id}", request.Id);
                    return GetSystemParameterByIdResponse.Failed($"System parameter with ID {request.Id} not found.");
                }

                var systemParameterDto = _mapper.Map<SystemParameterDTO>(systemParameter);

                _logger.LogInformation("Successfully retrieved system parameter details for ID: {Id}", request.Id);
                return GetSystemParameterByIdResponse.Success(systemParameterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting system parameter details for ID: {Id}", request.Id);
                return GetSystemParameterByIdResponse.Failed("An error occurred while retrieving system parameter details.");
            }
        }
    }
}