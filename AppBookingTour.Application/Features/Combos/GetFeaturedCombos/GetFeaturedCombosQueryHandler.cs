using AppBookingTour.Application.IRepositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Combos.GetFeaturedCombos;

public class GetFeaturedCombosQueryHandler : IRequestHandler<GetFeaturedCombosQuery, List<FeaturedComboDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetFeaturedCombosQueryHandler> _logger;

    public GetFeaturedCombosQueryHandler(IUnitOfWork unitOfWork, ILogger<GetFeaturedCombosQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<List<FeaturedComboDTO>> Handle(GetFeaturedCombosQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting {Count} featured combos", request.Count);

        // Get featured combos using repository method
        var combos = await _unitOfWork.Combos.GetFeaturedCombosAsync(request.Count, cancellationToken);

        if (combos == null || !combos.Any())
        {
            _logger.LogWarning("No featured combos found");
            return new List<FeaturedComboDTO>();
        }

        // Map to DTO
        var featuredCombos = combos
            .Select(c => new FeaturedComboDTO
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name,
                DurationDays = c.DurationDays,
                BasePriceAdult = c.BasePriceAdult,
                BasePriceChildren = c.BasePriceChildren,
                FromCityName = c.FromCity?.Name ?? "",
                ToCityName = c.ToCity?.Name ?? "",
                Vehicle = c.Vehicle,
                ComboImageCoverUrl = c.ComboImageCoverUrl,
                Rating = c.Rating,
                Schedules = c.Schedules
                    .OrderBy(s => s.DepartureDate)
                    .Select(s => new FeaturedComboScheduleItem
                    {
                        Id = s.Id,
                        DepartureDate = s.DepartureDate,
                        ReturnDate = s.ReturnDate,
                        AvailableSlots = s.AvailableSlots
                    })
                    .ToList()
            })
            .ToList();

        _logger.LogInformation("Retrieved {Count} featured combos", featuredCombos.Count);

        return featuredCombos;
    }
}
