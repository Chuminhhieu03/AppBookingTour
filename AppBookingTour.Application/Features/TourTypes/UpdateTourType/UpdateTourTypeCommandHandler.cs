using AppBookingTour.Application.Features.TourTypes.GetTourTypeById;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.TourTypes.UpdateTourType;

public sealed class UpdateTourTypeCommandHandler : IRequestHandler<UpdateTourTypeCommand, TourTypeDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateTourTypeCommandHandler> _logger;
    private readonly IMapper _mapper;

    public UpdateTourTypeCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateTourTypeCommandHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<TourTypeDTO> Handle(UpdateTourTypeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Tour type updating with ID: {TourTypeId}", request.TourTypeId);
        var existingTourType = await _unitOfWork.TourTypes.GetByIdAsync(request.TourTypeId, cancellationToken);
        if (existingTourType == null)
        {
            _logger.LogWarning("Tour type with ID {TourTypeId} not found.", request.TourTypeId);
            throw new KeyNotFoundException($"Tour type with ID {request.TourTypeId} not found.");
        }

        _mapper.Map(request.RequestDto, existingTourType);
        existingTourType.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var tourTypeDto = _mapper.Map<TourTypeDTO>(existingTourType);

        _logger.LogInformation("Tour type updated with ID: {TourTypeId}", request.TourTypeId);
        return tourTypeDto;
    }
}