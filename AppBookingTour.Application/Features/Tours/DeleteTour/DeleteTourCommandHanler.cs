using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Tours.DeleteTour;

#region Handler
public sealed class DeleteTourCommandHanler : IRequestHandler<DeleteTourCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteTourCommandHanler> _logger;
    public DeleteTourCommandHanler(
        IUnitOfWork unitOfWork,
       
    ILogger<DeleteTourCommandHanler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async Task<Unit> Handle(DeleteTourCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Delete tour by id: {TourId}", request.TourId);

        var existingTour = await _unitOfWork.Tours.GetByIdAsync(request.TourId, cancellationToken);
        if (existingTour == null)
        {
            throw new KeyNotFoundException($"Tour with ID {request.TourId} not found.");
        }

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            // Lấy các bản ghi con
            var tourDepartures = await _unitOfWork.Repository<TourDeparture>()
                .FindAsync(d => d.TourId == request.TourId, cancellationToken);

            var tourItineraries = await _unitOfWork.Repository<TourItinerary>()
                .FindAsync(i => i.TourId == request.TourId, cancellationToken);

            // Xóa các bản ghi con
            _unitOfWork.Repository<TourDeparture>().RemoveRange(tourDepartures);
            _unitOfWork.Repository<TourItinerary>().RemoveRange(tourItineraries);

            // Xóa bản ghi cha
            _unitOfWork.Tours.Remove(existingTour);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

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