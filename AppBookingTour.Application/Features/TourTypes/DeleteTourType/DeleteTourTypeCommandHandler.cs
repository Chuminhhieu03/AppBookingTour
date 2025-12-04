using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.IServices;
using AppBookingTour.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.TourTypes.DeleteTourType;

#region Handler
public sealed class DeleteTourTypeCommandHandler : IRequestHandler<DeleteTourTypeCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorageService;
    private readonly ILogger<DeleteTourTypeCommandHandler> _logger;

    public DeleteTourTypeCommandHandler(
        IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService,
        ILogger<DeleteTourTypeCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _fileStorageService = fileStorageService;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteTourTypeCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting tour type with ID: {TourTypeId}", request.TourTypeId);
        var existingTourType = await _unitOfWork.TourTypes.GetByIdAsync(request.TourTypeId, cancellationToken);

        if (existingTourType == null)
        {
            _logger.LogWarning("Tour type with ID: {TourTypeId} not found", request.TourTypeId);
            throw new KeyNotFoundException($"Tour type with ID {request.TourTypeId} not found.");
        }

        var oldImageUrl = existingTourType.ImageUrl;
        if (!string.IsNullOrEmpty(oldImageUrl))
        {
            await _fileStorageService.DeleteFileAsync(oldImageUrl);
        }

        _unitOfWork.TourTypes.Remove(existingTourType);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Tour type with ID: {TourTypeId} deleted successfully", request.TourTypeId);
        return Unit.Value;
    }
}
#endregion