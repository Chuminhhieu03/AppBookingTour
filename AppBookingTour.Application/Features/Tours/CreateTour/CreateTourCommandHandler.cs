using MediatR;
using Microsoft.Extensions.Logging;
using AppBookingTour.Domain.Entities;

using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.Features.Tours.GetTourById;
using AppBookingTour.Application.Features.TourItineraries.GetTourItineraryById;

namespace AppBookingTour.Application.Features.Tours.CreateTour;

#region Handler
public sealed class CreateTourCommandHandler : IRequestHandler<CreateTourCommand, CreateTourCommandResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateTourCommandHandler> _logger;

    public CreateTourCommandHandler(
        IUnitOfWork unitOfWork,
        ILogger<CreateTourCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<CreateTourCommandResponse> Handle(CreateTourCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Create tour from dto request");
        try
        {
            // TODO: dùng AutoMapper để map
            // map từ DTO => entity 
            var tour = new Tour
            {
                Code = request.TourRequest.Code!, //! Để đảm bảo là code sẽ k null khi đến đây (vì đã bắt ở validator)
                Name = request.TourRequest.Name!,
                TypeId = request.TourRequest.TypeId!.Value,
                CategoryId = request.TourRequest.CategoryId!.Value,
                DepartureCityId = request.TourRequest.DepartureCityId!.Value,
                DurationDays = request.TourRequest.DurationDays!.Value,
                DurationNights = request.TourRequest.DurationNights!.Value,
                MaxParticipants = request.TourRequest.MaxParticipants!.Value,
                MinParticipants = request.TourRequest.MinParticipants!.Value,
                BasePriceAdult = request.TourRequest.BasePriceAdult!.Value,
                BasePriceChild = request.TourRequest.BasePriceChild!.Value,
                Status = (Domain.Enums.TourStatus)request.TourRequest.Status!,
                IsActive = request.TourRequest.IsActive!.Value,
                ImageGallery = request.TourRequest.ImageGallery,
                Description = request.TourRequest.Description,
                Includes = request.TourRequest.Includes,
                Excludes = request.TourRequest.Excludes,
                TermsConditions = request.TourRequest.TermsConditions,
                Rating = 0,
                TotalBookings = 0,
                ViewCount = 0,
                InterestCount = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

           if(request.TourRequest.Itineraries != null)
            {
                foreach (var itineraryDto in request.TourRequest.Itineraries)
                {
                    var itinerary = new TourItinerary
                    { 
                        DayNumber = itineraryDto.DayNumber!.Value,
                        Title = itineraryDto.Title!,
                        Description = itineraryDto.Description,
                        Activity = itineraryDto.Activity,
                        Note = itineraryDto.Note
                    };
                    tour.Itineraries.Add(itinerary);
                }
            }

            if(request.TourRequest.TourDepartures != null)
            {
                foreach (var departureDto in request.TourRequest.TourDepartures)
                {
                    var departure = new TourDeparture
                    {
                        TourId = tour.Id,
                        DepartureDate = departureDto.DepartureDate,
                        ReturnDate = departureDto.ReturnDate,
                        AvailableSlots = departureDto.AvailableSlots,
                        BookedSlots = 0,
                        PriceAdult = departureDto.PriceAdult ?? tour.BasePriceAdult,
                        PriceChildren = departureDto.PriceChildren ?? tour.BasePriceChild,
                        Status = (Domain.Enums.DepartureStatus)departureDto.Status,
                        GuideId = departureDto.GuideId
                    };
                    tour.Departures.Add(departure);
                }
            }

            await _unitOfWork.Repository<Tour>().AddAsync(tour, cancellationToken);
            int records = await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (records == 0)
            {
                _logger.LogWarning("Create tour failed, no records affected");
                return CreateTourCommandResponse.Failed("Create tour failed, no records affected");
            }

            // TODO: dùng AutoMapper để map
            // map entity => dto
            var tourDto = new TourDetailDto
            {
                Id = tour.Id,
                Code = tour.Code,
                Name = tour.Name,
                DurationDays = tour.DurationDays,
                DurationNights = tour.DurationNights,
                BasePriceAdult = tour.BasePriceAdult,
                BasePriceChild = tour.BasePriceChild,
                MaxParticipants = tour.MaxParticipants,
                MinParticipants = tour.MinParticipants,
                Description = tour.Description,
                Includes = tour.Includes?.Split(',').ToList() ?? new List<string>(),
                Excludes = tour.Excludes?.Split(',').ToList() ?? new List<string>(),
                TermsConditions = tour.TermsConditions,
                ImageGallery = tour.ImageGallery?.Split(',').ToList() ?? new List<string>(),
                Rating = tour.Rating,
                TotalBookings = tour.TotalBookings,
                ViewCount = tour.ViewCount,
                IsActive = tour.IsActive,
                CreatedAt = tour.CreatedAt,
                UpdatedAt = tour.UpdatedAt,
                Itineraries = tour.Itineraries.Select(i => new TourItineraryDto
                {
                    DayNumber = i.DayNumber,
                    Title = i.Title,
                    Description = i.Description,
                    Activity = i.Activity,
                    Note = i.Note
                }).OrderBy(i => i.DayNumber).ToList(),
                TourDepartures = tour.Departures.Where(d => d.DepartureDate >= DateTime.UtcNow).Select(d => new TourDepartureDto
                {
                    Id = d.Id,
                    DepartureDate = d.DepartureDate,
                    ReturnDate = d.ReturnDate,
                    PriceAdult = d.PriceAdult,
                    PriceChildren = d.PriceChildren,
                    AvailableSlots = d.AvailableSlots - d.BookedSlots,
                    GuideName = d.Guide != null ? d.Guide.FullName : "N/A",
                    Status = d.Status.ToString()
                }).OrderBy(d => d.DepartureDate).ToList(),
            };
            return CreateTourCommandResponse.Success(tourDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tour");
            return CreateTourCommandResponse.Failed("Create tour failed" + ex);
        }
    }
}
#endregion
