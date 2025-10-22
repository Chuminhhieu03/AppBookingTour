using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.TourCategories.DeleteTourCategory;

public sealed class DeleteTourCategoryCommandHandler : IRequestHandler<DeleteTourCategoryCommand, DeleteTourCategoryResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteTourCategoryCommandHandler> _logger;

    public DeleteTourCategoryCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteTourCategoryCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<DeleteTourCategoryResponse> Handle(DeleteTourCategoryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Delete tour category by id: {TourCategoryId}", request.TourCategoryId);

        var existingCategory = await _unitOfWork.Repository<TourCategory>()
            .GetByIdAsync(request.TourCategoryId, cancellationToken);

        if (existingCategory == null)
        {
            _logger.LogWarning("Tour category with ID {TourCategoryId} not found.", request.TourCategoryId);
            return DeleteTourCategoryResponse.Failed($"Tour category with ID {request.TourCategoryId} not found.");
        }

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var childTours = await _unitOfWork.Tours.FindAsync(t => t.CategoryId == request.TourCategoryId, cancellationToken);
            var childTourIds = childTours.Select(t => t.Id).ToList();
            // Xóa toàn bộ lịch trình, lịch khởi hành và tour thuộc danh mục cần xóa
            if (childTourIds.Any())
            {
                var tourItineraries = await _unitOfWork.Repository<TourItinerary>()
                    .FindAsync(i => childTourIds.Contains(i.TourId), cancellationToken);
                _unitOfWork.Repository<TourItinerary>().RemoveRange(tourItineraries);

                var tourDepartures = await _unitOfWork.Repository<TourDeparture>()
                    .FindAsync(d => childTourIds.Contains(d.TourId), cancellationToken);
                _unitOfWork.Repository<TourDeparture>().RemoveRange(tourDepartures);

                _unitOfWork.Tours.RemoveRange(childTours);
            }

            var childCategories = await _unitOfWork.Repository<TourCategory>()
                .FindAsync(c => c.ParentCategoryId == request.TourCategoryId, cancellationToken);

            foreach (var child in childCategories)
            {
                child.ParentCategoryId = null;
                _unitOfWork.Repository<TourCategory>().Update(child);
            }

            _unitOfWork.Repository<TourCategory>().Remove(existingCategory);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            _logger.LogInformation("Tour category with ID: {TourCategoryId} and associated tours deleted successfully", request.TourCategoryId);
            return DeleteTourCategoryResponse.Success();
        }
        catch (DbUpdateException dbEx)
        {
            _logger.LogError(dbEx, "Error deleting category ID {TourCategoryId}. A child tour might be linked to a Booking.", request.TourCategoryId);
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return DeleteTourCategoryResponse.Failed("An error occurred. A tour in this category might be linked to an existing booking.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during transaction for deleting TourCategory ID {TourCategoryId}.", request.TourCategoryId);
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            return DeleteTourCategoryResponse.Failed("An error occurred while deleting the tour category.");
        }
    }
}