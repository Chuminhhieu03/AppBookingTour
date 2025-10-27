using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Combos.DeleteCombo;

public sealed class DeleteComboCommandHandler : IRequestHandler<DeleteComboCommand, DeleteComboResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteComboCommandHandler> _logger;

    public DeleteComboCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteComboCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<DeleteComboResponse> Handle(DeleteComboCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting combo with ID: {ComboId}", request.ComboId);
        try
        {
            var existingCombo = await _unitOfWork.Repository<Combo>().GetByIdAsync(request.ComboId, cancellationToken);
            if (existingCombo == null)
            {
                return DeleteComboResponse.Failed($"Combo with ID {request.ComboId} not found.");
            }

            await _unitOfWork.BeginTransactionAsync();

            var comboSchedules = await _unitOfWork.Repository<ComboSchedule>()
                .FindAsync(s => s.ComboId == request.ComboId, cancellationToken);

            _unitOfWork.Repository<ComboSchedule>().RemoveRange(comboSchedules);
            _unitOfWork.Repository<Combo>().Remove(existingCombo);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync();

            return DeleteComboResponse.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting combo with ID {ComboId}", request.ComboId);
            await _unitOfWork.RollbackTransactionAsync();
            return DeleteComboResponse.Failed("An error occurred while deleting the combo.");
        }
    }
}