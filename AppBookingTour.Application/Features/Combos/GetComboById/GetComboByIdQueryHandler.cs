using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Enums;

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
        // Sử dụng ComboRepository để load combo với details
        var combo = await _unitOfWork.Combos.GetComboWithDetailsAsync(request.ComboId, cancellationToken);

        if (combo == null)
        {
            _logger.LogWarning("Combo not found with ID: {ComboId}", request.ComboId);
            return GetComboByIdResponse.Failed($"Combo with ID {request.ComboId} not found");
        }

        // Get List Image by ComboID sử dụng ImageRepository
        var lstImage = await _unitOfWork.Images.GetListImageByEntityIdAndEntityType(combo.Id, EntityType.Combo);

        var comboDto = _mapper.Map<ComboDTO>(combo);
        comboDto.ComboImages = [.. lstImage.Select(x => x.Url)];

        return GetComboByIdResponse.Success(comboDto);
    }
}