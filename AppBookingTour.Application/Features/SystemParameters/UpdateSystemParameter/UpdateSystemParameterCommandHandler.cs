using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.SystemParameters.UpdateSystemParameter
{
    public sealed class UpdateSystemParameterCommandHandler : IRequestHandler<UpdateSystemParameterCommand, UpdateSystemParameterResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateSystemParameterCommandHandler> _logger;
        private readonly IMapper _mapper;

        public UpdateSystemParameterCommandHandler(
            IUnitOfWork unitOfWork,
            ILogger<UpdateSystemParameterCommandHandler> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<UpdateSystemParameterResponse> Handle(UpdateSystemParameterCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating system parameter with ID: {Id}", request.Id);
            try
            {
                var existingSystemParameter = await _unitOfWork.Repository<SystemParameter>().GetByIdAsync(request.Id, cancellationToken);

                if (existingSystemParameter == null)
                {
                    _logger.LogWarning("System parameter with ID {Id} not found.", request.Id);
                    return UpdateSystemParameterResponse.Failed($"System parameter with ID {request.Id} not found.");
                }

                _mapper.Map(request.RequestDto, existingSystemParameter);
                existingSystemParameter.UpdatedAt = DateTime.UtcNow;

                _unitOfWork.Repository<SystemParameter>().Update(existingSystemParameter);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("System parameter updated successfully with ID: {Id}", request.Id);
                return UpdateSystemParameterResponse.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating system parameter with ID: {Id}", request.Id);
                return UpdateSystemParameterResponse.Failed("An error occurred while updating the system parameter.");
            }
        }
    }
}