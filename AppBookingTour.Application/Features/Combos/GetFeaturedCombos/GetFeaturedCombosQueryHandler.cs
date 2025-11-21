using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
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

        // Get all active combos with related data
        var combos = await _unitOfWork.Repository<Combo>()
            .GetAllAsync(cancellationToken: cancellationToken);

        if (combos == null || !combos.Any())
        {
            _logger.LogWarning("No active combos found");
            return new List<FeaturedComboDTO>();
        }

        // Get random n combos
        var randomCombos = combos
            .OrderBy(x => Guid.NewGuid())
            .Take(request.Count)
            .Select(c => new FeaturedComboDTO
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name,
                FromCityName = c.FromCity?.Name ?? "Unknown",
                ToCityName = c.ToCity?.Name ?? "Unknown",
                ShortDescription = c.ShortDescription,
                Vehicle = "",
                ComboImageCoverUrl = c.ComboImageCoverUrl,
                DurationDays = c.DurationDays,
                BasePriceAdult = c.BasePriceAdult,
                Rating = c.Rating
            })
            .ToList();

        _logger.LogInformation("Retrieved {Count} featured combos", randomCombos.Count);

        return randomCombos;
    }
}
