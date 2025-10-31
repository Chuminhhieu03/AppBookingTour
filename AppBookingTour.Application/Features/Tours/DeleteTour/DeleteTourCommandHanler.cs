using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.IServices;
using AppBookingTour.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Tours.DeleteTour;

#region Handler
public sealed class DeleteTourCommandHanler : IRequestHandler<DeleteTourCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorageService _fileStorageService;
    private readonly ILogger<DeleteTourCommandHanler> _logger;
    public DeleteTourCommandHanler(
        IUnitOfWork unitOfWork,
        IFileStorageService fileStorageService,
        ILogger<DeleteTourCommandHanler> logger)
    {
        _unitOfWork = unitOfWork;
        _fileStorageService = fileStorageService;
        _logger = logger;
    }
    public async Task<Unit> Handle(DeleteTourCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Delete tour by id: {TourId}", request.TourId);

        var existingTour = await _unitOfWork.Tours.GetByIdAsync(request.TourId, c => c.Departures, c => c.Itineraries);
        if (existingTour == null)
        {
            throw new KeyNotFoundException($"Tour with ID {request.TourId} not found.");
        }

        var imagesToDelete = await _unitOfWork.Images.GetListImageByEntityIdAndEntityType(request.TourId, Domain.Enums.EntityType.Tour);

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            // Lấy các bản ghi con
            var tourDepartures = existingTour.Departures.ToList();
            var tourItineraries = existingTour.Itineraries.ToList();

            // Xóa các bản ghi con
            _unitOfWork.Repository<TourDeparture>().RemoveRange(tourDepartures);
            _unitOfWork.Repository<TourItinerary>().RemoveRange(tourItineraries);

            // Xóa hình ảnh liên quan
            if (imagesToDelete != null)
            {
                _unitOfWork.Images.RemoveRange(imagesToDelete);
            }

            // Xóa bản ghi cha
            _unitOfWork.Tours.Remove(existingTour);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            // Xóa file hình ảnh khỏi lưu trữ
            if (imagesToDelete != null)
            {
                foreach (var img in imagesToDelete)
                {
                    await _fileStorageService.DeleteFileAsync(img.Url);
                }
            }

            _logger.LogInformation("Tour {TourId} deleted successfully", request.TourId);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            _logger.LogError(ex, "Error deleting tour {TourId}", request.TourId);
            throw;
        }

        return Unit.Value;
    }
}
#endregion