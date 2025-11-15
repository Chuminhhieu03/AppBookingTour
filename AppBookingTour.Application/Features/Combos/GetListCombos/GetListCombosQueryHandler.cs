using MediatR;
using Microsoft.Extensions.Logging;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.Combos.GetListCombos;

public sealed class GetListCombosQueryHandler : IRequestHandler<GetListCombosQuery, PagedResult<ComboListDTO>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetListCombosQueryHandler> _logger;

    public GetListCombosQueryHandler(IUnitOfWork unitOfWork, ILogger<GetListCombosQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PagedResult<ComboListDTO>> Handle(GetListCombosQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting list of combos with filters");
        
        var req = request.Request;

        var allCombos = await _unitOfWork.Repository<Combo>().GetAllAsync(cancellationToken);
        var query = allCombos.AsQueryable();

        if (req.FromCityId.HasValue)
        {
            query = query.Where(c => c.FromCityId == req.FromCityId.Value);
        }

        if (req.ToCityId.HasValue)
        {
            query = query.Where(c => c.ToCityId == req.ToCityId.Value);
        }

        if (req.Vehicle.HasValue)
        {
            query = query.Where(c => (int)c.Vehicle == req.Vehicle.Value);
        }

        if (req.IsActive.HasValue)
        {
            query = query.Where(c => c.IsActive == req.IsActive.Value);
        }

        if (req.MinPrice.HasValue)
        {
            query = query.Where(c => c.BasePriceAdult >= req.MinPrice.Value);
        }

        if (req.MaxPrice.HasValue)
        {
            query = query.Where(c => c.BasePriceAdult <= req.MaxPrice.Value);
        }

        if (req.MinDuration.HasValue)
        {
            query = query.Where(c => c.DurationDays >= req.MinDuration.Value);
        }

        if (req.MaxDuration.HasValue)
        {
            query = query.Where(c => c.DurationDays <= req.MaxDuration.Value);
        }

        if (!string.IsNullOrWhiteSpace(req.SearchTerm))
        {
            var searchTerm = req.SearchTerm.ToLower();
            query = query.Where(c => c.Name.ToLower().Contains(searchTerm) || c.Code.ToLower().Contains(searchTerm));
        }

        var totalCount = query.Count();

        query = ApplySorting(query, req.SortBy, req.SortDescending);

        var pagedCombos = query.Skip(req.PageIndex * req.PageSize).Take(req.PageSize).ToList();

        var comboIds = pagedCombos.Select(c => c.Id).ToList();
        
        var allCities = await _unitOfWork.Repository<City>().GetAllAsync(cancellationToken);
        var cityDict = allCities.ToDictionary(c => c.Id, c => c);

        var allSchedules = await _unitOfWork.Repository<ComboSchedule>().FindAsync(s => comboIds.Contains(s.ComboId), cancellationToken);
        var schedulesDict = allSchedules.GroupBy(s => s.ComboId).ToDictionary(g => g.Key, g => g.ToList());

        var items = pagedCombos.Select(c => new ComboListDTO
        {
            Id = c.Id,
            Code = c.Code,
            Name = c.Name,
            FromCityId = c.FromCityId,
            FromCityName = cityDict.ContainsKey(c.FromCityId) ? cityDict[c.FromCityId].Name : "N/A",
            ToCityId = c.ToCityId,
            ToCityName = cityDict.ContainsKey(c.ToCityId) ? cityDict[c.ToCityId].Name : "N/A",
            ShortDescription = c.ShortDescription,
            Vehicle = (int)c.Vehicle,
            DurationDays = c.DurationDays,
            BasePriceAdult = c.BasePriceAdult,
            BasePriceChildren = c.BasePriceChildren,
            ComboImageCoverUrl = c.ComboImageCoverUrl,
            Rating = c.Rating,
            TotalBookings = c.TotalBookings,
            ViewCount = c.ViewCount,
            IsActive = c.IsActive,
            AvailableSchedulesCount = schedulesDict.ContainsKey(c.Id) ? schedulesDict[c.Id].Count(s => s.Status == Domain.Enums.ComboStatus.Available) : 0,
            NextDepartureDate = schedulesDict.ContainsKey(c.Id) ? schedulesDict[c.Id].Where(s => s.Status == Domain.Enums.ComboStatus.Available && s.DepartureDate > DateTime.UtcNow).OrderBy(s => s.DepartureDate).Select(s => (DateTime?)s.DepartureDate).FirstOrDefault() : null
        }).ToList();

        _logger.LogInformation("Retrieved {Count} combos out of {TotalCount}", items.Count, totalCount);

        return new PagedResult<ComboListDTO>
        {
            Items = items,
            TotalCount = totalCount,
            PageIndex = req.PageIndex,
            PageSize = req.PageSize
        };
    }

    private IQueryable<Combo> ApplySorting(IQueryable<Combo> query, string? sortBy, bool descending)
    {
        return sortBy?.ToLower() switch
        {
            "price" => descending ? query.OrderByDescending(c => c.BasePriceAdult) : query.OrderBy(c => c.BasePriceAdult),
            "rating" => descending ? query.OrderByDescending(c => c.Rating) : query.OrderBy(c => c.Rating),
            "totalbookings" => descending ? query.OrderByDescending(c => c.TotalBookings) : query.OrderBy(c => c.TotalBookings),
            "createdat" => descending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt),
            "name" => descending ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name),
            _ => descending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt)
        };
    }
}
