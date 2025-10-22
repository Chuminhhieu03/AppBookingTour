using AppBookingTour.Application.IRepositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Tours.GetTourById;

#region Handler
public sealed class GetTourByIdQueryHandler : IRequestHandler<GetTourByIdQuery, GetTourByIdResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetTourByIdQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetTourByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetTourByIdQueryHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<GetTourByIdResponse> Handle(GetTourByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting tour details for ID: {TourId}", request.TourId);

        try
        {
            var tour = await _unitOfWork.Tours.GetTourWithDetailsAsync(request.TourId, cancellationToken);

            if (tour == null)
            {
                _logger.LogWarning("Tour not found with ID: {TourId}", request.TourId);
                return GetTourByIdResponse.Failed($"Tour with ID {request.TourId} not found");
            }

            // TODO: update lại cái này
            tour.ViewCount++;
            _unitOfWork.Tours.Update(tour);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var tourDto = _mapper.Map<TourDTO>(tour);

            _logger.LogInformation("Successfully retrieved tour details for ID: {TourId}", request.TourId);
            return GetTourByIdResponse.Success(tourDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting tour details for ID: {TourId}", request.TourId);
            return GetTourByIdResponse.Failed("An error occurred while retrieving tour details");
        }
    }
}
#endregion