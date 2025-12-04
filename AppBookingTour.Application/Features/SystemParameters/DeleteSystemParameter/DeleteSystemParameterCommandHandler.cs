using AppBookingTour.Application.Features.SystemParameters.DeleteSystemParameter;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.SystemParameters.Commands.DeleteSystemParameter
{
    public sealed class DeleteSystemParameterCommandHandler : IRequestHandler<DeleteSystemParameterCommand, DeleteSystemParameterResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteSystemParameterCommandHandler> _logger;

        public DeleteSystemParameterCommandHandler(
            IUnitOfWork unitOfWork,
            ILogger<DeleteSystemParameterCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<DeleteSystemParameterResponse> Handle(DeleteSystemParameterCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting system parameter with ID: {Id}", request.Id);
            try
            {
                var systemParameterToDelete = await _unitOfWork.Repository<SystemParameter>().GetByIdAsync(request.Id, cancellationToken);

                if (systemParameterToDelete == null)
                {
                    _logger.LogWarning("System parameter with ID: {Id} not found", request.Id);
                    return DeleteSystemParameterResponse.Failed($"System parameter with ID {request.Id} not found.");
                }

                _unitOfWork.Repository<SystemParameter>().Remove(systemParameterToDelete);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("System parameter with ID: {Id} deleted successfully", request.Id);
                return DeleteSystemParameterResponse.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting system parameter with ID: {Id}", request.Id);
                return DeleteSystemParameterResponse.Failed("An error occurred while deleting the system parameter.");
            }
        }
    }
}