// AppBookingTour.Application/Features/ComboSchedules/GetComboScheduleById/GetComboScheduleByIdQueryHandler.cs
using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;

using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;

namespace AppBookingTour.Application.Features.ComboSchedules.GetComboScheduleById;

public sealed class GetComboScheduleByIdQueryHandler : IRequestHandler<GetComboScheduleByIdQuery, GetComboScheduleByIdResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetComboScheduleByIdQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetComboScheduleByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetComboScheduleByIdQueryHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<GetComboScheduleByIdResponse> Handle(GetComboScheduleByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting combo schedule details for ID: {ComboScheduleId}", request.ComboScheduleId);
        try
        {
            var comboSchedule = await _unitOfWork.Repository<ComboSchedule>().GetByIdAsync(request.ComboScheduleId, cancellationToken);

            if (comboSchedule == null)
            {
                _logger.LogWarning("Combo schedule not found with ID: {ComboScheduleId}", request.ComboScheduleId);
                return GetComboScheduleByIdResponse.Failed($"Combo schedule with ID {request.ComboScheduleId} not found");
            }

            var comboScheduleDto = _mapper.Map<ComboScheduleDTO>(comboSchedule);

            _logger.LogInformation("Successfully retrieved combo schedule for ID: {ComboScheduleId}", request.ComboScheduleId);
            return GetComboScheduleByIdResponse.Success(comboScheduleDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting combo schedule with ID: {ComboScheduleId}", request.ComboScheduleId);
            return GetComboScheduleByIdResponse.Failed("An error occurred while processing your request.");
        }
    }
}