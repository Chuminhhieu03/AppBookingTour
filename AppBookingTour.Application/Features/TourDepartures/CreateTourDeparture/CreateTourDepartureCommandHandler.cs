using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;

using AppBookingTour.Domain.Entities;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.Features.TourDepartures.GetTourDepartureById;


namespace AppBookingTour.Application.Features.TourDepartures.CreateTourDeparture;

#region Handler
public sealed class CreateTourDepartureCommandHandler : IRequestHandler<CreateTourDepartureCommand, CreateTourDepartureResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateTourDepartureCommandHandler> _logger;
    private readonly IMapper _mapper;
    public CreateTourDepartureCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateTourDepartureCommandHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task<CreateTourDepartureResponse> Handle(CreateTourDepartureCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Create tour departure from dto request");
        try
        {
            var tourDeparture = _mapper.Map<TourDeparture>(request.TourDepartureRequest);
            tourDeparture.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<TourDeparture>().AddAsync(tourDeparture, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Tour departure created successfully with ID: {TourDepartureId}", tourDeparture.Id);
            var tourDepartureDto = _mapper.Map<TourDepartureDTO>(tourDeparture);

            return CreateTourDepartureResponse.Success(tourDepartureDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating tour departure");
            return CreateTourDepartureResponse.Failed("An error occurred while creating the tour departure.");
        }
    }
}
#endregion
