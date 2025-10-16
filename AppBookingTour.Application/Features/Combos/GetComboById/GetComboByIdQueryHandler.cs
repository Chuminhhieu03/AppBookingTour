using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;

using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;

namespace AppBookingTour.Application.Features.Combos.GetComboById;

public sealed class GetComboByIdQueryHandler : IRequestHandler<GetComboByIdQuery, GetComboByIdResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetComboByIdQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetComboByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetComboByIdQueryHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<GetComboByIdResponse> Handle(GetComboByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting combo details for ID: {ComboId}", request.ComboId);
        try
        {
            var combo = await _unitOfWork.Repository<Combo>().GetByIdAsync(request.ComboId,
                c => c.FromCity,
                c => c.ToCity,
                c => c.Schedules);

            if (combo == null)
            {
                _logger.LogWarning("Combo not found with ID: {ComboId}", request.ComboId);
                return GetComboByIdResponse.Failed($"Combo with ID {request.ComboId} not found");
            }

            var comboDto = _mapper.Map<ComboDTO>(combo);

            return GetComboByIdResponse.Success(comboDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting combo details for ID: {ComboId}", request.ComboId);
            return GetComboByIdResponse.Failed("An error occurred while retrieving combo details");
        }
    }
}