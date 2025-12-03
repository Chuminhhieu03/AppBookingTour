using MediatR;
using Microsoft.Extensions.Logging;
using AutoMapper;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;

namespace AppBookingTour.Application.Features.TourDepartures.GetListTourDepartureForGuide
{
    public sealed class GetListTourForGuideQueryHandler : IRequestHandler<GetListTourDepartureForGuideQuery, List<TourDepartureItemForGuide>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetListTourForGuideQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetListTourForGuideQueryHandler(
            IUnitOfWork unitOfWork,
            ILogger<GetListTourForGuideQueryHandler> logger,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<List<TourDepartureItemForGuide>> Handle(GetListTourDepartureForGuideQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting all tour departures for GuideId: {GuideId}", request.GuideId);

            var tourDepartures = await _unitOfWork.Repository<TourDeparture>()
                .FindAsync(
                    td => td.GuideId == request.GuideId,
                    td => td.Tour,
                    td => td.Tour.DepartureCity,
                    td => td.Tour.DestinationCity
                );

            if (!tourDepartures.Any())
            {
                _logger.LogInformation("No tour departures found for GuideId: {GuideId}", request.GuideId);
                return new List<TourDepartureItemForGuide>();
            }

            var result = _mapper.Map<List<TourDepartureItemForGuide>>(tourDepartures)
                .OrderBy(td => td.DepartureDate)
                .ToList();

            _logger.LogInformation("Retrieved {Count} tour departures for GuideId: {GuideId}", result.Count, request.GuideId);

            return result;
        }
    }
}
