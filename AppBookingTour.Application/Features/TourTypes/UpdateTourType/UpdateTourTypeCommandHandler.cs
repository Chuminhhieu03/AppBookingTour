using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.TourTypes.UpdateTourType;

public sealed class UpdateTourTypeCommandHandler : IRequestHandler<UpdateTourTypeCommand, UpdateTourTypeResponse>
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

    public async Task<UpdateTourTypeResponse> Handle(UpdateTourTypeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Tour type updating with ID: {TourTypeId}", request.TourTypeId);
        try
        {
            var existingTourType = await _unitOfWork.Repository<TourType>().GetByIdAsync(request.TourTypeId, cancellationToken);
            if (existingTourType == null)
            {
                _logger.LogWarning("Tour type with ID {TourTypeId} not found.", request.TourTypeId);
                return UpdateTourTypeResponse.Failed($"Tour type with ID {request.TourTypeId} not found.");
            }

            _mapper.Map(request.RequestDto, existingTourType);

            existingTourType.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Tour type updated with ID: {TourTypeId}", request.TourTypeId);

            return UpdateTourTypeResponse.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating tour type.");
            return UpdateTourTypeResponse.Failed("An error occurred while updating the tour type.");
        }
    }
}