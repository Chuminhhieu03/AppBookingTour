using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Domain.Entities;

public class Review : BaseEntity
{
    public int? ReviewParentId { get; set; }
    public int UserId { get; set; }
    public int BookingId { get; set; }
    public int ItemId { get; set; }
    public ReviewItemType ItemType { get; set; }
    public int Rating { get; set; } // 1-10
    public string? Title { get; set; }
    public string? Comment { get; set; }
    public DateTime ReviewDate { get; set; }
    public ReviewStatus Status { get; set; } = ReviewStatus.Pending;
    public int HelpfulCount { get; set; } = 0;

    // Navigation properties
    public virtual Review? ParentReview { get; set; }
    public virtual ICollection<Review> Replies { get; set; } = [];
    public virtual User User { get; set; } = null!;
    public virtual Booking Booking { get; set; } = null!;
}

