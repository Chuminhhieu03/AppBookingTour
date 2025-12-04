using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Application.Features.Reviews.CreateReview;

public class CreateReviewRequest
{
    public int UserId { get; set; }
    public int BookingId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
}

public class CreateReviewResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public ReviewDto? Review { get; set; }
}

public class ReviewDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int BookingId { get; set; }
    public int ItemId { get; set; }
    public ReviewItemType ItemType { get; set; }
    public string ItemTypeName { get; set; } = "";
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime ReviewDate { get; set; }
    public ReviewStatus Status { get; set; }
    public string StatusName { get; set; } = "";
}
