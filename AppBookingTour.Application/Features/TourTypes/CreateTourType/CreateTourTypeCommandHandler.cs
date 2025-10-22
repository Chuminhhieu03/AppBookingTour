using AppBookingTour.Application.Features.TourTypes.GetTourTypeById;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.TourTypes.CreateTourType;

public sealed class CreateTourTypeCommandHandler : IRequestHandler<CreateTourTypeCommand, CreateTourTypeResponse>
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

    public async Task<CreateTourTypeResponse> Handle(CreateTourTypeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating a new tour type");
        try
        {
            var tourType = _mapper.Map<TourType>(request.RequestDto);

            tourType.CreatedAt = DateTime.UtcNow;
            tourType.IsActive = request.RequestDto.IsActive ?? true;

            await _unitOfWork.Repository<TourType>().AddAsync(tourType, cancellationToken);
            var records = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (records == 0)
            {
                _logger.LogWarning("Create tour type failed, no records affected");
                return CreateTourTypeResponse.Fail("Create tour type failed, no records affected");
            }

            var tourTypeDto = _mapper.Map<TourTypeDTO>(tourType);

            _logger.LogInformation("Tour type created successfully with ID: {TourTypeId}", tourType.Id);
            return CreateTourTypeResponse.Success(tourTypeDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tour type");
            return CreateTourTypeResponse.Fail("Create tour type failed: " + ex.Message);
        }
    }
}