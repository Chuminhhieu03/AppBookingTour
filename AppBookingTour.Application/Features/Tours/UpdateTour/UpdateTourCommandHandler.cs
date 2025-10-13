using MediatR;
using Microsoft.Extensions.Logging;
using AppBookingTour.Application.IRepositories;

namespace AppBookingTour.Application.Features.Tours.UpdateTour;

#region Handler
public sealed class UpdateTourComandHandler : IRequestHandler<UpdateTourCommand, UpdateTourCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateTourComandHandler> _logger;

    public UpdateTourComandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateTourComandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<UpdateTourCommandResponse> Handle(UpdateTourCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Tour updating with ID: {TourId}", request.TourId);
        try
        {
            var existingTour = await _unitOfWork.Tours.GetByIdAsync(request.TourId, cancellationToken);
            if (existingTour == null)
            {
                return UpdateTourCommandResponse.Failed($"Tour with ID {request.TourId} not found.");
            }

            existingTour.Code = request.TourRequest.Code ?? existingTour.Code;
            existingTour.Name = request.TourRequest.Name ?? existingTour.Name;
            existingTour.TypeId = request.TourRequest.TypeId ?? existingTour.TypeId;
            existingTour.CategoryId = request.TourRequest.CategoryId ?? existingTour.CategoryId;
            existingTour.DepartureCityId = request.TourRequest.DepartureCityId ?? existingTour.DepartureCityId;
            existingTour.DurationDays = request.TourRequest.DurationDays ?? existingTour.DurationDays;
            existingTour.DurationNights = request.TourRequest.DurationNights ?? existingTour.DurationNights;
            existingTour.MaxParticipants = request.TourRequest.MaxParticipants ?? existingTour.MaxParticipants;
            existingTour.MinParticipants = request.TourRequest.MinParticipants ?? existingTour.MinParticipants;
            existingTour.BasePriceAdult = request.TourRequest.BasePriceAdult ?? existingTour.BasePriceAdult;
            existingTour.BasePriceChild = request.TourRequest.BasePriceChild ?? existingTour.BasePriceChild;
            existingTour.Status = request.TourRequest.Status != null ? (Domain.Enums.TourStatus)request.TourRequest.Status : existingTour.Status;
            existingTour.IsActive = request.TourRequest.IsActive ?? existingTour.IsActive;
            existingTour.ImageGallery = request.TourRequest.ImageGallery ?? existingTour.ImageGallery;
            existingTour.Description = request.TourRequest.Description ?? existingTour.Description;
            existingTour.Includes = request.TourRequest.Includes ?? existingTour.Includes;
            existingTour.Excludes = request.TourRequest.Excludes ?? existingTour.Excludes;
            existingTour.TermsConditions = request.TourRequest.TermsConditions ?? existingTour.TermsConditions;
            existingTour.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Tour updated with ID: {TourId}", request.TourId);

            return UpdateTourCommandResponse.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating tour.");
            return UpdateTourCommandResponse.Failed("An error occurred while updating the tour.");
        }
    }
};
#endregion
