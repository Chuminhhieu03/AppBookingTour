using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Enums;
using AppBookingTour.Application.Features.TourItineraries.GetTourItineraryById;

namespace AppBookingTour.Application.Features.Tours.GetTourById;
#region Validator
#endregion

#region Handler
public sealed class GetTourByIdQueryHandler : IRequestHandler<GetTourByIdQuery, GetTourByIdResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetTourByIdQueryHandler> _logger;

    public GetTourByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetTourByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
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

            // Increment view count (side effect) => chuẩn bị bỏ
            tour.ViewCount++;
            _unitOfWork.Tours.Update(tour);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // TODO: dùng AutoMapper để map
            // Map entity => dto
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
                Rating = tour.Rating,
                TotalBookings = tour.TotalBookings,
                ViewCount = tour.ViewCount,
                //ImageMain = tour.ImageMain,
                ImageGallery = ParseJsonArray(tour.ImageGallery) ?? [],
                IsActive = tour.IsActive,
                CreatedAt = tour.CreatedAt,
                UpdatedAt = tour.UpdatedAt,
                DepartureCityName = tour.DepartureCity?.Name ?? "Unknown",
                TypeName = tour.Type?.Name ?? "Unknown",
                Description = tour.Description,
                Includes = ParseJsonArray(tour.Includes) ?? [],
                Excludes = ParseJsonArray(tour.Excludes) ?? [],
                TermsConditions = tour.TermsConditions,
                Itineraries = tour.Itineraries?
                    .OrderBy(i => i.DayNumber)
                    .Select(i => new TourItineraryDto
                    {
                        DayNumber = i.DayNumber,
                        Title = i.Title,
                        Description = i.Description,
                        Activity = i.Activity,
                        Note = i.Note
                    })
                    .ToList() ?? [],
                TourDepartures = tour.Departures?
                    .Where(d => d.DepartureDate > DateTime.Now && d.Status == DepartureStatus.Available)
                    .OrderBy(d => d.DepartureDate)
                    .Take(5)
                    .Select(d => new TourDepartureDto
                    {
                        Id = d.Id,
                        DepartureDate = d.DepartureDate,
                        ReturnDate = d.ReturnDate,
                        PriceAdult = d.PriceAdult,
                        PriceChildren = d.PriceChildren,
                        AvailableSlots = d.AvailableSlots,
                        GuideName = d.Guide?.FullName,
                        Status = d.Status.ToString()
                    })
                    .ToList() ?? []
            };

            _logger.LogInformation("Successfully retrieved tour details for ID: {TourId}", request.TourId);
            return GetTourByIdResponse.Success(tourDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting tour details for ID: {TourId}", request.TourId);
            return GetTourByIdResponse.Failed("An error occurred while retrieving tour details");
        }
    }

    private static List<string>? ParseJsonArray(string? jsonString)
    {
        if (string.IsNullOrEmpty(jsonString))
            return null;

        try
        {
            return JsonSerializer.Deserialize<List<string>>(jsonString);
        }
        catch
        {
            // Fallback: try to split by comma
            return jsonString.Split(',')
                .Select(s => s.Trim(' ', '"', '[', ']'))
                .Where(s => !string.IsNullOrEmpty(s))
                .ToList();
        }
    }
}
#endregion