using AppBookingTour.Application.Features.SystemParameters.GetSystemParameterById;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.SystemParameters.CreateSystemParameter
{
    public sealed class CreateSystemParameterCommandHandler : IRequestHandler<CreateSystemParameterCommand, CreateSystemParameterResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateSystemParameterCommandHandler> _logger;
        private readonly IMapper _mapper;

        public CreateSystemParameterCommandHandler(
            IUnitOfWork unitOfWork,
            ILogger<CreateSystemParameterCommandHandler> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<CreateSystemParameterResponse> Handle(CreateSystemParameterCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating a new system parameter");
            try
            {

                var systemParameter = _mapper.Map<SystemParameter>(request.RequestDto);
                systemParameter.CreatedAt = DateTime.UtcNow;

                await _unitOfWork.Repository<SystemParameter>().AddAsync(systemParameter, cancellationToken);
                var recordsAffected = await _unitOfWork.SaveChangesAsync(cancellationToken);

                if (recordsAffected == 0)
                {
                    _logger.LogWarning("Create system parameter failed, no records affected");
                    return CreateSystemParameterResponse.Fail("Create system parameter failed, no records affected");
                }

                var systemParameterDto = _mapper.Map<SystemParameterDTO>(systemParameter);

                _logger.LogInformation("System parameter created successfully with ID: {Id}", systemParameter.Id);
                return CreateSystemParameterResponse.Success(systemParameterDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating system parameter");
                return CreateSystemParameterResponse.Fail("An error occurred while creating the system parameter.");
            }
        }
    }
}