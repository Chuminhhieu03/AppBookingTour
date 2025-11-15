using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

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
        
        // Load combo sử dụng ComboRepository
        var existingCombo = await _unitOfWork.Combos.GetByIdAsync(request.ComboId, cancellationToken);

        if (existingCombo == null)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return UpdateComboResponse.Failed($"Combo với ID {request.ComboId} không tồn tại");
        }

        // Kiểm tra combo có booking nào chưa sử dụng ComboRepository
        var hasBookings = await _unitOfWork.Combos.HasActiveBookingsAsync(request.ComboId, cancellationToken);

        if (hasBookings)
        {
            // Nếu có booking, chỉ cho phép update một số field
            _logger.LogWarning("Combo {ComboId} has active bookings. Limited update only.", request.ComboId);
            
            existingCombo.Description = request.ComboRequest.Description ?? existingCombo.Description;
            existingCombo.Includes = request.ComboRequest.Includes ?? existingCombo.Includes;
            existingCombo.Excludes = request.ComboRequest.Excludes ?? existingCombo.Excludes;
            existingCombo.TermsConditions = request.ComboRequest.TermsConditions ?? existingCombo.TermsConditions;
            existingCombo.IsActive = request.ComboRequest.IsActive ?? existingCombo.IsActive;
        }
        else
        {
            // Nếu chưa có booking, cho phép update toàn bộ
            _mapper.Map(request.ComboRequest, existingCombo);
            
            // Xử lý schedules nếu có update
            if (request.ComboRequest.Schedules != null && request.ComboRequest.Schedules.Any())
            {
                // Load và xóa schedules cũ
                var existingSchedules = await _unitOfWork.Repository<ComboSchedule>()
                    .FindAsync(s => s.ComboId == request.ComboId, cancellationToken);
                
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