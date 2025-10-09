using AppBookingTour.Application.Features.Tours.GetTourById;
using AppBookingTour.Application.IRepositories;
using MediatR;
using Microsoft.Extensions.Logging;
using AppBookingTour.Domain.Entities;

namespace AppBookingTour.Application.Features.Tours.CreateTour;

#region Handler
public sealed class CreateTourCommandHandler : IRequestHandler<CreateTourCommand, GetTourByIdResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateTourCommandHandler> _logger;

    public CreateTourCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateTourCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<GetTourByIdResponse> Handle(CreateTourCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Create tour from dto request");
        try
        {
            // map từ DTO => entity 
            var tour = new Tour
            {
                Code = request.TourRequest.Code,
                Name = request.TourRequest.Name,
                TypeId = request.TourRequest.TypeId,
                CategoryId = request.TourRequest.CategoryId,
                DepartureCityId = request.TourRequest.DepartureCityId,
                DurationDays = request.TourRequest.DurationDays,
                DurationNights = request.TourRequest.DurationNights,
                MaxParticipants = request.TourRequest.MaxParticipants,
                MinParticipants = request.TourRequest.MinParticipants,
                BasePriceAdult = request.TourRequest.BasePriceAdult,
                BasePriceChild = request.TourRequest.BasePriceChild,
                Status = (Domain.Enums.TourStatus)request.TourRequest.Status,
                IsActive = request.TourRequest.IsActive,
                Rating = 0,
                TotalBookings = 0,
                ViewCount = 0,
                InterestCount = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };
            // gọi unitOfWork.Tours.Add(entity)
            await _unitOfWork.Repository<Tour>().AddAsync(tour, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return GetTourByIdResponse.Failed("Tour created suscessfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tour");
            return GetTourByIdResponse.Failed("An error occurred while creating the tour.");
        }
    }
}
#endregion
