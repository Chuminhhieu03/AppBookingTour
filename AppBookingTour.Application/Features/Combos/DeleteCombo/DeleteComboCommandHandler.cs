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
        
        var existingCombo = await _unitOfWork.Repository<Combo>()
            .GetByIdAsync(request.ComboId, cancellationToken);
            
        if (existingCombo == null)
        {
            return DeleteComboResponse.Failed($"Combo với ID {request.ComboId} không tồn tại");
        }

        // Kiểm tra combo có booking nào chưa
        var hasActiveBookings = await _unitOfWork.Repository<Booking>()
            .ExistsAsync(b => b.BookingType == Domain.Enums.BookingType.Combo 
                && b.ItemId == request.ComboId 
                && (b.Status == Domain.Enums.BookingStatus.Confirmed 
                    || b.Status == Domain.Enums.BookingStatus.Paid
                    || b.Status == Domain.Enums.BookingStatus.Pending), 
                cancellationToken);

        if (hasActiveBookings)
        {
            return DeleteComboResponse.Failed("Không thể xóa combo đã có booking active. Vui lòng hủy tất cả booking trước.");
        }

        await _unitOfWork.BeginTransactionAsync();

        // Soft delete: set IsActive = false thay vì xóa hẳn
        existingCombo.IsActive = false;
        existingCombo.UpdatedAt = DateTime.UtcNow;

        // Cũng set status = Cancelled cho tất cả schedules
        var comboSchedules = await _unitOfWork.Repository<ComboSchedule>()
            .FindAsync(s => s.ComboId == request.ComboId, cancellationToken);

        foreach (var schedule in comboSchedules)
        {
            schedule.Status = Domain.Enums.ComboStatus.Cancelled;
            schedule.UpdatedAt = DateTime.UtcNow;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _unitOfWork.CommitTransactionAsync();

        _logger.LogInformation("Successfully soft deleted combo with ID: {ComboId}", request.ComboId);
        return DeleteComboResponse.Success();
    }
}