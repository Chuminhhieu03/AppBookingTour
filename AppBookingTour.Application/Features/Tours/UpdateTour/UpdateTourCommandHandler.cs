using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;

using AppBookingTour.Application.IRepositories;

namespace AppBookingTour.Application.Features.Tours.UpdateTour;

#region Handler
public sealed class UpdateTourComandHandler : IRequestHandler<UpdateTourCommand, UpdateTourResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateTourComandHandler> _logger;
    private readonly IMapper _mapper;

    public UpdateTourComandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateTourComandHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<UpdateTourResponse> Handle(UpdateTourCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Tour updating with ID: {TourId}", request.TourId);
        try
        {
            var existingTour = await _unitOfWork.Tours.GetByIdAsync(request.TourId, cancellationToken);
            if (existingTour == null)
            {
                return UpdateTourResponse.Failed($"Tour with ID {request.TourId} not found.");
            }

            _mapper.Map(request.TourRequest, existingTour);
            existingTour.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Tour updated with ID: {TourId}", request.TourId);

            return UpdateTourResponse.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating tour.");
            return UpdateTourResponse.Failed("An error occurred while updating the tour.");
        }
    }
};
#endregion
