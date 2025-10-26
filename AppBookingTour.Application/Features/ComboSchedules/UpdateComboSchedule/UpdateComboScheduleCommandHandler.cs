using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;

using AppBookingTour.Application.IRepositories;

namespace AppBookingTour.Application.Features.ComboSchedules.UpdateComboSchedule;

public sealed class UpdateComboScheduleCommandHandler : IRequestHandler<UpdateComboScheduleCommand, UpdateComboScheduleResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateComboScheduleCommandHandler> _logger;
    private readonly IMapper _mapper;

    public UpdateComboScheduleCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateComboScheduleCommandHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<UpdateComboScheduleResponse> Handle(UpdateComboScheduleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating combo schedule with ID: {ComboScheduleId}", request.ComboScheduleId);
        try
        {
            var existingSchedule = await _unitOfWork.Repository<Domain.Entities.ComboSchedule>()
                .GetByIdAsync(request.ComboScheduleId, cancellationToken);

            if (existingSchedule == null)
            {
                return UpdateComboScheduleResponse.Failed($"Combo schedule with ID {request.ComboScheduleId} not found.");
            }

            _mapper.Map(request.ComboScheduleRequest, existingSchedule);
            existingSchedule.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Combo schedule updated with ID: {ComboScheduleId}", request.ComboScheduleId);
            return UpdateComboScheduleResponse.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating combo schedule with ID: {ComboScheduleId}", request.ComboScheduleId);
            return UpdateComboScheduleResponse.Failed("An error occurred while updating the combo schedule.");
        }
    }
}