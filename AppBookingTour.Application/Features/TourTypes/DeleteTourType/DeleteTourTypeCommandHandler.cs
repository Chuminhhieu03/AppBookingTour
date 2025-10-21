using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.TourTypes.DeleteTourType;

#region Handler
public sealed class DeleteTourTypeCommandHandler : IRequestHandler<DeleteTourTypeCommand, DeleteTourTypeResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteTourTypeCommandHandler> _logger;

    public DeleteTourTypeCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteTourTypeCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<DeleteTourTypeResponse> Handle(DeleteTourTypeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting tour type with ID: {TourTypeId}", request.TourTypeId);
        try
        {
            var tourType = await _unitOfWork.Repository<TourType>().GetByIdAsync(request.TourTypeId, cancellationToken);

            if (tourType == null)
            {
                _logger.LogWarning("Tour type with ID: {TourTypeId} not found", request.TourTypeId);
                return DeleteTourTypeResponse.Failed("Tour type not found.");
            }

            _unitOfWork.Repository<TourType>().Remove(tourType);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Tour type with ID: {TourTypeId} deleted successfully", request.TourTypeId);
            return DeleteTourTypeResponse.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting tour type with ID: {TourTypeId}. It may be in use.", request.TourTypeId);
            return DeleteTourTypeResponse.Failed("An error occurred. This tour type might be in use by existing tours.");
        }
    }
}
#endregion