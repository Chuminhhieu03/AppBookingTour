using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Tours.GetFeaturedTours;

public class GetFeaturedToursQueryHandler : IRequestHandler<GetFeaturedToursQuery, List<FeaturedTourDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetFeaturedToursQueryHandler> _logger;

    public GetFeaturedToursQueryHandler(IUnitOfWork unitOfWork, ILogger<GetFeaturedToursQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<FeaturedTourDTO>> Handle(GetFeaturedToursQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting {Count} featured tours", request.Count);

        // Get all active tours with related data
        var tours = await _unitOfWork.Repository<Tour>()
            .GetAllAsync(cancellationToken: cancellationToken);

        if (tours == null || !tours.Any())
        {
            _logger.LogWarning("No active tours found");
            return new List<FeaturedTourDTO>();
        }

        // Get random n tours
        var randomTours = tours
            .OrderBy(x => Guid.NewGuid())
            .Take(request.Count)
            .Select(t => new FeaturedTourDTO
            {
                Id = t.Id,
                Code = t.Code,
                Name = t.Name,
                Description = t.Description,
                DurationDays = t.DurationDays,
                DurationNights = t.DurationNights,
                BasePriceAdult = t.BasePriceAdult,
                Rating = t.Rating,
                ImageMainUrl = t.ImageMainUrl,
                DepartureCityName = t.DepartureCity?.Name ?? "Unknown",
                DestinationCityName = t.DestinationCity?.Name ?? "Unknown",
                TypeName = t.Type?.Name ?? "Unknown"
            })
            .ToList();

        _logger.LogInformation("Retrieved {Count} featured tours", randomTours.Count);

        return randomTours;
    }
}
