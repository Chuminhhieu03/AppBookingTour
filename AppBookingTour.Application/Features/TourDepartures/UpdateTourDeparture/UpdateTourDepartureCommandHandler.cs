using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;

using AppBookingTour.Application.IRepositories;

namespace AppBookingTour.Application.Features.TourDepartures.UpdateTourDeparture;

#region Handler
public sealed class UpdateTourDepartureCommandHandler : IRequestHandler<UpdateTourDepartureCommand, UpdateTourDepartureResponse>
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
    public async Task<UpdateTourDepartureResponse> Handle(UpdateTourDepartureCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating tour departure with ID: {TourDepartureId}", request.TourDepartureId);
        try
        {
            var existingDeparture = await _unitOfWork.Repository<Domain.Entities.TourDeparture>()
                .GetByIdAsync(request.TourDepartureId, cancellationToken);
            if (existingDeparture == null)
            {
                return UpdateTourDepartureResponse.Failed($"Tour departure with ID {request.TourDepartureId} not found.");
            }

            // Map updated fields from request to existing entity
            _mapper.Map(request.TourDepartureRequest, existingDeparture);
            existingDeparture.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Tour departure updated with ID: {TourDepartureId}", request.TourDepartureId);
            return UpdateTourDepartureResponse.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tour departure with ID: {TourDepartureId}", request.TourDepartureId);
            return UpdateTourDepartureResponse.Failed("An error occurred while updating the tour departure.");
        }
    }
}
#endregion
