using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;

using AppBookingTour.Domain.Entities;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.Features.Tours.GetTourById;

namespace AppBookingTour.Application.Features.Tours.CreateTour;

#region Handler
public sealed class CreateTourCommandHandler : IRequestHandler<CreateTourCommand, CreateTourResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateTourCommandHandler> _logger;
    private readonly IMapper _mapper;

    public CreateTourCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateTourCommandHandler> logger,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<CreateTourResponse> Handle(CreateTourCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating a new tour using AutoMapper");
        try
        {
            var tour = _mapper.Map<Tour>(request.TourRequest);

            tour.Rating = 0;
            tour.TotalBookings = 0;
            tour.ViewCount = 0;
            tour.InterestCount = 0;
            tour.CreatedAt = DateTime.UtcNow;

            foreach (var departure in tour.Departures)
            {
                departure.BookedSlots = 0;
            }

            await _unitOfWork.Repository<Tour>().AddAsync(tour, cancellationToken);
            int records = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (records == 0)
            {
                _logger.LogWarning("Create tour failed, no records affected");
                return CreateTourResponse.Failed("Create tour failed, no records affected");
            }


            var tourDto = _mapper.Map<TourDTO>(tour);

            tourDto.Itineraries = tourDto.Itineraries.OrderBy(i => i.DayNumber).ToList();
            tourDto.Departures = tourDto.Departures
                                          .OrderBy(d => d.DepartureDate)
                                          .ToList();


            return CreateTourResponse.Success(tourDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tour");
            return CreateTourResponse.Failed("Create tour failed: " + ex.Message);
        }
    }
}
#endregion
