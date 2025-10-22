using AppBookingTour.Application.Features.TourTypes.GetTourTypeById;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.TourTypes.GetTourTypesList;

public sealed class GetTourTypesListQueryHandler : IRequestHandler<GetTourTypesListQuery, GetTourTypesListResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetTourTypesListQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetTourTypesListQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetTourTypesListQueryHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<GetTourTypesListResponse> Handle(GetTourTypesListQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all tour types");
        try
        {
            var tourTypes = await _unitOfWork.Repository<TourType>().GetAllAsync(cancellationToken);

            var tourTypeDtos = _mapper.Map<List<TourTypeDTO>>(tourTypes);

            tourTypeDtos = tourTypeDtos.OrderBy(t => t.Name).ToList();

            return GetTourTypesListResponse.Success(tourTypeDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all tour types");
            return GetTourTypesListResponse.Failed("An error occurred while retrieving tour types.");
        }
    }
}