using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace AppBookingTour.Application.Features.Combos.UpdateCombo;

public sealed class UpdateComboCommandHandler : IRequestHandler<UpdateComboCommand, UpdateComboResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateComboCommandHandler> _logger;
    private readonly IMapper _mapper;

    public UpdateComboCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateComboCommandHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<UpdateComboResponse> Handle(UpdateComboCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating combo with ID: {ComboId}", request.ComboId);
        
        await _unitOfWork.BeginTransactionAsync();
        
        // Load combo
        var existingCombo = await _unitOfWork.Repository<Combo>()
            .FirstOrDefaultAsync(c => c.Id == request.ComboId, cancellationToken);

        if (existingCombo == null)
        {
            return UpdateComboResponse.Failed($"Combo với ID {request.ComboId} không tồn tại");
        }

        // Kiểm tra combo có booking nào chưa
        var hasBookings = await _unitOfWork.Repository<Booking>()
            .ExistsAsync(b => b.BookingType == Domain.Enums.BookingType.Combo 
                && b.ItemId == request.ComboId 
                && b.Status != Domain.Enums.BookingStatus.Cancelled, 
                cancellationToken);

        // Nếu chưa có booking, cho phép update toàn bộ
        _mapper.Map(request.ComboRequest, existingCombo);
            
        // Xử lý schedules nếu có update
        if (request.ComboRequest.Schedules != null && request.ComboRequest.Schedules.Any())
        {
            // Load và xóa schedules cũ
            var existingSchedules = await _unitOfWork.Repository<ComboSchedule>()
                .FindAsync(s => s.ComboId == request.ComboId, cancellationToken);

            // Xóa schedules cũ
            if (existingSchedules.Any())
            {
                _unitOfWork.Repository<ComboSchedule>().RemoveRange(existingSchedules);
            }
                
            // Thêm schedules mới
            var newSchedules = new List<ComboSchedule>();
            foreach (var scheduleDto in request.ComboRequest.Schedules)
            {
                var schedule = _mapper.Map<ComboSchedule>(scheduleDto);
                schedule.ComboId = existingCombo.Id;
                schedule.BookedSlots = 0;
                schedule.Status = Domain.Enums.ComboStatus.Available;
                schedule.CreatedAt = DateTime.UtcNow;
                newSchedules.Add(schedule);
            }
            await _unitOfWork.Repository<ComboSchedule>().AddRangeAsync(newSchedules, cancellationToken);
        }

        existingCombo.UpdatedAt = DateTime.UtcNow;

        var recordsAffected = await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        if (recordsAffected == 0)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return UpdateComboResponse.Failed("Không có thay đổi nào được lưu");
        }

        await _unitOfWork.CommitTransactionAsync();

        _logger.LogInformation("Successfully updated combo with ID: {ComboId}", request.ComboId);
        return UpdateComboResponse.Success();
    }
}