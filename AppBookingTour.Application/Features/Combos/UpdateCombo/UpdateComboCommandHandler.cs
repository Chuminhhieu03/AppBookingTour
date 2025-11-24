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
        
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        // Load combo sử dụng ComboRepository
        var existingCombo = await _unitOfWork.Combos.GetByIdAsync(request.ComboId, cancellationToken);

        if (existingCombo == null)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return UpdateComboResponse.Failed($"Combo với ID {request.ComboId} không tồn tại");
        }

        // Mapping dữ liệu 
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

            // Câp nhât trang thái cho từng schedule nếu không có thì sẽ xét là 1
            foreach(var schedule in existingCombo.Schedules)
            {
                schedule.Status = Domain.Enums.ComboStatus.Available;
            }
        }

        // Cập nhật thời gian update 
        existingCombo.UpdatedAt = DateTime.UtcNow;



        var recordsAffected = await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        if (recordsAffected == 0)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return UpdateComboResponse.Failed("Không có thay đổi nào được lưu");
        }

        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        _logger.LogInformation("Successfully updated combo with ID: {ComboId}", request.ComboId);
        return UpdateComboResponse.Success();
    }
}