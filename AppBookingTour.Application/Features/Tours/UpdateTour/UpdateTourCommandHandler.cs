using AppBookingTour.Application.Features.Tours.GetTourById;
using AppBookingTour.Application.IRepositories;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Tours.UpdateTour;

#region Handler
public sealed class UpdateTourComandHandler : IRequestHandler<UpdateTourCommand, TourDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateTourComandHandler> _logger;
    private readonly IMapper _mapper;

    public UpdateTourComandHandler(
        IUnitOfWork unitOfWork,
        ILogger<UpdateTourComandHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<TourDTO> Handle(UpdateTourCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Tour updating with ID: {TourId}", request.TourId);
        var existingTour = await _unitOfWork.Tours.GetByIdAsync(request.TourId, cancellationToken);
        if (existingTour == null)
        {
            throw new KeyNotFoundException($"Tour with ID {request.TourId} not found.");
        }

        _mapper.Map(request.TourRequest, existingTour);
        existingTour.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Tour updated with ID: {TourId}", request.TourId);
        var tourDto = _mapper.Map<TourDTO>(existingTour);
        return tourDto;
    }
};
#endregion
