using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;

using AppBookingTour.Domain.Entities;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.Features.ComboSchedules.GetComboScheduleById;

namespace AppBookingTour.Application.Features.ComboSchedules.CreateComboSchedule;

public sealed class CreateComboScheduleCommandHandler : IRequestHandler<CreateComboScheduleCommand, CreateComboScheduleResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateComboScheduleCommandHandler> _logger;
    private readonly IMapper _mapper;

    public CreateComboScheduleCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateComboScheduleCommandHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<CreateComboScheduleResponse> Handle(CreateComboScheduleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating a new combo schedule");
        try
        {
            var comboSchedule = _mapper.Map<ComboSchedule>(request.ComboScheduleRequest);

            comboSchedule.BookedSlots = 0;
            comboSchedule.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<ComboSchedule>().AddAsync(comboSchedule, cancellationToken);
            int records = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (records == 0)
            {
                _logger.LogWarning("Create combo schedule failed, no records affected");
                return CreateComboScheduleResponse.Failed("Create combo schedule failed, no records affected");
            }

            var comboScheduleDto = _mapper.Map<ComboScheduleDTO>(comboSchedule);

            return CreateComboScheduleResponse.Success(comboScheduleDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating combo schedule");
            return CreateComboScheduleResponse.Failed("Create combo schedule failed: " + ex.Message);
        }
    }
}