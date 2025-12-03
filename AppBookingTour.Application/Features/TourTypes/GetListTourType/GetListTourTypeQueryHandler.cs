using AppBookingTour.Application.Features.TourTypes.GetTourTypeById;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.TourTypes.GetTourTypesList;

public sealed class GetTourTypesListQueryHandler : IRequestHandler<GetTourTypesListQuery, List<TourTypeDTO>>
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

    public async Task<List<TourTypeDTO>> Handle(GetTourTypesListQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting list tour type active!");
        var tourTypes = await _unitOfWork.TourTypes.FindAsync(x => x.IsActive, cancellationToken);

        var tourTypeDtos = _mapper.Map<List<TourTypeDTO>>(tourTypes);

        tourTypeDtos = tourTypeDtos.OrderBy(t => t.Name).ToList();

        return tourTypeDtos;
    }
}