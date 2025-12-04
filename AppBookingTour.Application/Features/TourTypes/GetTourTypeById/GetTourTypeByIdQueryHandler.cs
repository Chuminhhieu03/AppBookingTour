using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.TourTypes.GetTourTypeById;

public sealed class GetTourTypeByIdQueryHandler : IRequestHandler<GetTourTypeByIdQuery, TourTypeDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetTourTypeByIdQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetTourTypeByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetTourTypeByIdQueryHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<TourTypeDTO> Handle(GetTourTypeByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting tour type details for ID: {TourTypeId}", request.TourTypeId);
        var tourType = await _unitOfWork.TourTypes.GetByIdAsync(request.TourTypeId, cancellationToken);

        if (tourType == null)
        {
            _logger.LogWarning("Tour type not found with ID: {TourTypeId}", request.TourTypeId);
            throw new KeyNotFoundException($"Tour type with ID {request.TourTypeId} not found.");
        }

        var tourTypeDto = _mapper.Map<TourTypeDTO>(tourType);

        _logger.LogInformation("Successfully retrieved tour type details for ID: {TourTypeId}", request.TourTypeId);
        return tourTypeDto;
    }
}