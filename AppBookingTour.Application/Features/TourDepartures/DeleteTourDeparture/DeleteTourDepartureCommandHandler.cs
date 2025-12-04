using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;

using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;

namespace AppBookingTour.Application.Features.TourDepartures.DeleteTourDeparture;

#region Handler
public sealed class DeleteTourDepartureCommandHandler : IRequestHandler<DeleteTourDepartureCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteTourDepartureCommandHandler> _logger;
    private readonly IMapper _mapper;
    public DeleteTourDepartureCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<DeleteTourDepartureCommandHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task<Unit> Handle(DeleteTourDepartureCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting tour departure with ID: {Id}", request.Id);
        var tourDeparture = await _unitOfWork.Repository<TourDeparture>().GetByIdAsync(request.Id, cancellationToken);
        if (tourDeparture == null)
        {
            _logger.LogWarning("Tour departure with ID: {Id} not found", request.Id);
            throw new KeyNotFoundException($"Tour departure with ID {request.Id} not found.");
        }

        _unitOfWork.Repository<TourDeparture>().Remove(tourDeparture);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Tour departure with ID: {Id} deleted successfully", request.Id);
        return Unit.Value;
    }
}
#endregion
