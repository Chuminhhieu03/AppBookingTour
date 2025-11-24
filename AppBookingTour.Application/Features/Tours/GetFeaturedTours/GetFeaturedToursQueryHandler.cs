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

        // Get featured tours using repository method
        var tours = await _unitOfWork.Tours.GetFeaturedToursAsync(request.Count, cancellationToken);

        if (tours == null || !tours.Any())
        {
            _logger.LogWarning("No featured tours found");
            return new List<FeaturedTourDTO>();
        }

        // Map to DTO
        var featuredTours = tours
            .Select(t => new FeaturedTourDTO
            {
                Id = t.Id,
                Code = t.Code,
                Name = t.Name,
                DurationDays = t.DurationDays,
                DurationNights = t.DurationNights,
                BasePriceAdult = t.BasePriceAdult,
                BasePriceChild = t.BasePriceChild,
                DepartureCityName = t.DepartureCity?.Name ?? "",
                DestinationCityName = t.DestinationCity?.Name ?? "",
                TypeId = t.TypeId,
                TypeName = t.Type?.Name ?? "",
                CategoryId = t.CategoryId,
                CategoryName = t.Category?.Name ?? "",
                ImageMainUrl = t.ImageMainUrl,
                Departures = t.Departures
                    .OrderBy(d => d.DepartureDate)
                    .Select(d => new FeaturedTourDepartureItem
                    {
                        Id = d.Id,
                        DepartureDate = d.DepartureDate,
                        ReturnDate = d.ReturnDate,
                        AvailableSlots = d.AvailableSlots
                    })
                    .ToList()
            })
            .ToList();

        _logger.LogInformation("Retrieved {Count} featured tours", featuredTours.Count);

        return featuredTours;
    }
}
