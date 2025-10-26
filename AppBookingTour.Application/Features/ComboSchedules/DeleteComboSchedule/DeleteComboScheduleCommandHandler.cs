using MediatR;

using Microsoft.Extensions.Logging;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;

namespace AppBookingTour.Application.Features.ComboSchedules.DeleteComboSchedule;

public sealed class DeleteComboScheduleCommandHandler : IRequestHandler<DeleteComboScheduleCommand, DeleteComboScheduleResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteComboScheduleCommandHandler> _logger;

    public DeleteComboScheduleCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteComboScheduleCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<DeleteComboScheduleResponse> Handle(DeleteComboScheduleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting combo schedule with ID: {ComboScheduleId}", request.ComboScheduleId);
        try
        {
            var comboSchedule = await _unitOfWork.Repository<ComboSchedule>().GetByIdAsync(request.ComboScheduleId, cancellationToken);

            if (comboSchedule == null)
            {
                _logger.LogWarning("Combo schedule with ID: {ComboScheduleId} not found", request.ComboScheduleId);
                return DeleteComboScheduleResponse.Failed($"Combo schedule with ID {request.ComboScheduleId} not found");
            }

            _unitOfWork.Repository<ComboSchedule>().Remove(comboSchedule);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Combo schedule with ID: {ComboScheduleId} deleted successfully", request.ComboScheduleId);
            return DeleteComboScheduleResponse.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting combo schedule with ID: {ComboScheduleId}", request.ComboScheduleId);
            return DeleteComboScheduleResponse.Failed("An error occurred while deleting the combo schedule.");
        }
    }
}