using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;

using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;

namespace AppBookingTour.Application.Features.TourDepartures.GetTourDepartureById;

#region Handler
public sealed class GetTourDepartureByIdQueryHandler : IRequestHandler<GetTourDepartureByIdQuery, GetTourDepartureByIdResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetTourDepartureByIdQueryHandler> _logger;
    private readonly IMapper _mapper;
    public GetTourDepartureByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetTourDepartureByIdQueryHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task<GetTourDepartureByIdResponse> Handle(GetTourDepartureByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting tour departure details for ID: {TourDepartureId}", request.TourDepartureId);
        try
        {
            var departure = await _unitOfWork.Repository<TourDeparture>().GetByIdAsync(request.TourDepartureId, cancellationToken);
            if (departure == null)
            {
                _logger.LogWarning("Tour departure not found with ID: {TourDepartureId}", request.TourDepartureId);
                return GetTourDepartureByIdResponse.Failed($"Tour departure with ID {request.TourDepartureId} not found");
            }

            var departureDto = _mapper.Map<TourDepartureDTO>(departure);

            _logger.LogInformation("Successfully retrieved departure for ID: {TourDepartureId}", request.TourDepartureId);
            return GetTourDepartureByIdResponse.Success(departureDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting tour departure with ID: {TourDepartureId}", request.TourDepartureId);
            return GetTourDepartureByIdResponse.Failed("An error occurred while processing your request.");
        }
    }
}
#endregion