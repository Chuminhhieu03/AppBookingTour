using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.TourCategories.DeleteTourCategory;

public sealed class DeleteTourCategoryCommandHandler : IRequestHandler<DeleteTourCategoryCommand, Unit>
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

    public async Task<Unit> Handle(DeleteTourCategoryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Delete tour category by id: {TourCategoryId}", request.TourCategoryId);

        var existingCategory = await _unitOfWork.TourCategories
            .GetByIdAsync(request.TourCategoryId, cancellationToken);

        if (existingCategory == null)
        {
            _logger.LogWarning("Tour category with ID {TourCategoryId} not found.", request.TourCategoryId);
            throw new KeyNotFoundException($"Tour category with ID {request.TourCategoryId} not found.");
        }

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

        var childCategories = await _unitOfWork.TourCategories
            .FindAsync(c => c.ParentCategoryId == request.TourCategoryId, cancellationToken);

        foreach (var child in childCategories)
        {
            child.ParentCategoryId = null;
            _unitOfWork.TourCategories.Update(child);
        }

        _unitOfWork.TourCategories.Remove(existingCategory);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        _logger.LogInformation("Tour category with ID: {TourCategoryId} and associated tours deleted successfully", request.TourCategoryId);
        return Unit.Value;
    }
}