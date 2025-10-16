using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Tours.DeleteTour;

#region Handler
public sealed class DeleteTourCommandHanler : IRequestHandler<DeleteTourCommand, DeleteTourResponse>
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
    public async Task<DeleteTourResponse> Handle(DeleteTourCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Delete tour by id: {TourId}", request.TourId);
        try
        {
            var existingTour = await _unitOfWork.Tours.GetByIdAsync(request.TourId, cancellationToken);

            if (existingTour == null)
            {
                return DeleteTourResponse.Failed($"Tour with ID {request.TourId} not found.");
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

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

                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await _unitOfWork.RollbackTransactionAsync();
                return DeleteTourResponse.Failed("An error occurred while deleting the tour. Please try again later.");
            }

            return DeleteTourResponse.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tour with ID {TourId}: {ErrorMessage}", request.TourId, ex.Message);
            return DeleteTourResponse.Failed("An error occurred while deleting the tour. Please try again later.");
        }
    }
}
#endregion