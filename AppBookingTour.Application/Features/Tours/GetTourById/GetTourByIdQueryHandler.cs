using AppBookingTour.Application.IRepositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Tours.GetTourById;

#region Handler
public sealed class GetTourByIdQueryHandler : IRequestHandler<GetTourByIdQuery, TourDTO>
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

    public async Task<TourDTO> Handle(GetTourByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting tour details for ID: {TourId}", request.TourId);
        var tour = await _unitOfWork.Tours.GetTourWithDetailsAsync(request.TourId, cancellationToken);

        if (tour == null)
        {
            throw new KeyNotFoundException($"Tour with ID {request.TourId} not found.");
        }

        // TODO: update lại cái này
        tour.ViewCount++;
        _unitOfWork.Tours.Update(tour);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var tourDto = _mapper.Map<TourDTO>(tour);

        _logger.LogInformation("Successfully retrieved tour details for ID: {TourId}", request.TourId);
        return tourDto;
    }
}
#endregion