using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;

using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.Features.TourDepartures.GetTourDepartureById;

namespace AppBookingTour.Application.Features.TourDepartures.UpdateTourDeparture;

#region Handler
public sealed class UpdateTourDepartureCommandHandler : IRequestHandler<UpdateTourDepartureCommand, TourDepartureDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateTourDepartureCommandHandler> _logger;
    private readonly IMapper _mapper;
    public UpdateTourDepartureCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateTourDepartureCommandHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task<TourDepartureDTO> Handle(UpdateTourDepartureCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating tour departure with ID: {TourDepartureId}", request.TourDepartureId);
        var existingDeparture = await _unitOfWork.Repository<Domain.Entities.TourDeparture>()
               .GetByIdAsync(request.TourDepartureId, cancellationToken);
        if (existingDeparture == null)
        {
            _logger.LogWarning("Tour departure with ID {TourDepartureId} not found.", request.TourDepartureId);
            throw new KeyNotFoundException($"Tour departure with ID {request.TourDepartureId} not found.");
        }

        // Map updated fields from request to existing entity
        _mapper.Map(request.TourDepartureRequest, existingDeparture);
        existingDeparture.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var updatedDepartureDto = _mapper.Map<TourDepartureDTO>(existingDeparture);

        _logger.LogInformation("Tour departure updated with ID: {TourDepartureId}", request.TourDepartureId);
        return updatedDepartureDto;
    }
}
#endregion
