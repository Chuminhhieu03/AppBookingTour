using AppBookingTour.Application.Features.TourTypes.GetTourTypeById;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.TourTypes.CreateTourType;

public sealed class CreateTourTypeCommandHandler : IRequestHandler<CreateTourTypeCommand, TourTypeDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateTourTypeCommandHandler> _logger;
    private readonly IMapper _mapper;

    public CreateTourTypeCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateTourTypeCommandHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<TourTypeDTO> Handle(CreateTourTypeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating a new tour type");
        var tourType = _mapper.Map<TourType>(request.RequestDto);

        tourType.CreatedAt = DateTime.UtcNow;
        tourType.IsActive = request.RequestDto.IsActive ?? true;

        await _unitOfWork.TourTypes.AddAsync(tourType, cancellationToken);
        var records = await _unitOfWork.SaveChangesAsync(cancellationToken);

        var tourTypeDto = _mapper.Map<TourTypeDTO>(tourType);

        _logger.LogInformation("Tour type created successfully with ID: {TourTypeId}", tourType.Id);
        return tourTypeDto;
    }
}