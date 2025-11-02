using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;

using AppBookingTour.Domain.Entities;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.Features.Combos.GetComboById;

namespace AppBookingTour.Application.Features.Combos.CreateCombo;

public sealed class CreateComboCommandHandler : IRequestHandler<CreateComboCommand, CreateComboResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateComboCommandHandler> _logger;
    private readonly IMapper _mapper;

    public CreateComboCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateComboCommandHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<CreateComboResponse> Handle(CreateComboCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating a new combo with code: {Code}", request.ComboRequest.Code);
        
        await _unitOfWork.BeginTransactionAsync();
        
        // Kiểm tra FromCity và ToCity tồn tại
        var fromCity = await _unitOfWork.Repository<City>()
            .GetByIdAsync(request.ComboRequest.FromCityId!.Value, cancellationToken);
        if (fromCity == null)
        {
            return CreateComboResponse.Failed($"Thành phố xuất phát với ID {request.ComboRequest.FromCityId} không tồn tại");
        }

        var toCity = await _unitOfWork.Repository<City>()
            .GetByIdAsync(request.ComboRequest.ToCityId!.Value, cancellationToken);
        if (toCity == null)
        {
            return CreateComboResponse.Failed($"Thành phố đến với ID {request.ComboRequest.ToCityId} không tồn tại");
        }

        // Kiểm tra Code trùng
        var codeExists = await _unitOfWork.Repository<Combo>()
            .ExistsAsync(c => c.Code == request.ComboRequest.Code, cancellationToken);
        if (codeExists)
        {
            return CreateComboResponse.Failed($"Mã combo '{request.ComboRequest.Code}' đã tồn tại");
        }

        // Map từ DTO sang Entity
        var combo = _mapper.Map<Combo>(request.ComboRequest);

        // Set default values
        combo.Rating = 0;
        combo.TotalBookings = 0;
        combo.ViewCount = 0;
        combo.InterestCount = 0;
        combo.IsActive = request.ComboRequest.IsActive ?? true;
        combo.CreatedAt = DateTime.UtcNow;

        // Xử lý Schedules
        if (combo.Schedules != null && combo.Schedules.Any())
        {
            foreach (var schedule in combo.Schedules)
            {
                schedule.BookedSlots = 0;
                schedule.Status = Domain.Enums.ComboStatus.Available;
                schedule.CreatedAt = DateTime.UtcNow;
                
                // Validate duration
                var duration = (schedule.ReturnDate - schedule.DepartureDate).Days;
                if (duration != combo.DurationDays)
                {
                    _logger.LogWarning("Schedule duration ({ScheduleDuration}) does not match combo duration ({ComboDuration})", 
                        duration, combo.DurationDays);
                }
            }

            // Sắp xếp schedules theo departure date
            combo.Schedules = combo.Schedules.OrderBy(s => s.DepartureDate).ToList();
        }

        // Lưu vào database
        await _unitOfWork.Repository<Combo>().AddAsync(combo, cancellationToken);
        var recordsAffected = await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (recordsAffected == 0)
        {
            await _unitOfWork.RollbackTransactionAsync();
            return CreateComboResponse.Failed("Không thể tạo combo. Vui lòng thử lại.");
        }

        await _unitOfWork.CommitTransactionAsync();

        // Load lại combo với navigation properties
        var createdCombo = await _unitOfWork.Repository<Combo>()
            .FirstOrDefaultAsync(c => c.Id == combo.Id, cancellationToken);

        if (createdCombo != null)
        {
            // Load navigation properties manually
            createdCombo.FromCity = fromCity;
            createdCombo.ToCity = toCity;
        }

        var comboDto = _mapper.Map<ComboDTO>(createdCombo);
        if (comboDto.Schedules != null)
        {
            comboDto.Schedules = comboDto.Schedules.OrderBy(s => s.DepartureDate).ToList();
        }

        _logger.LogInformation("Successfully created combo with ID: {ComboId}", combo.Id);
        return CreateComboResponse.Success(comboDto);
    }
}