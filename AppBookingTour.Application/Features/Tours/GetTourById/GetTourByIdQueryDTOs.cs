namespace AppBookingTour.Application.Features.Tours.GetTourById;

// Response DTOs - integrated in use case
public class GetTourByIdResponse
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public TourDetailDto? Tour { get; init; }

    public static GetTourByIdResponse Success(TourDetailDto tour) =>
        new() { IsSuccess = true, Tour = tour };

    public static GetTourByIdResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, ErrorMessage = errorMessage };
}

public class TourDetailDto
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int DurationDays { get; set; }
    public decimal BasePriceAdult { get; set; }
    public decimal BasePriceChild { get; set; }
    public int MaxParticipants { get; set; }
    public decimal? Rating { get; set; }
    public int TotalBookings { get; set; }
    public int ViewCount { get; set; }
    public string? ImageUrl { get; set; }
    public List<string> ImageGallery { get; set; } = [];
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Related data
    public string DepartureCityName { get; set; } = null!;
    public string TypeName { get; set; } = null!;
    public string? CategoryName { get; set; }

    // Detailed information
    public List<TourItineraryDto> Itineraries { get; set; } = [];
    public List<TourDepartureDto> UpcomingDepartures { get; set; } = [];
    public List<string> Includes { get; set; } = [];
    public List<string> Excludes { get; set; } = [];
    public string? TermsConditions { get; set; }
}

public class TourItineraryDto
{
    public string DayNumber { get; set; } = null!;
    public string? Title { get; set; }
    public string? Description { get; set; }
    public List<string> Activities { get; set; } = [];
    public string? Notes { get; set; }
}

public class TourDepartureDto
{
    public int Id { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public decimal PriceAdult { get; set; }
    public decimal PriceChildren { get; set; }
    public int AvailableSlots { get; set; }
    public string? GuideName { get; set; }
    public string Status { get; set; } = null!;
}