namespace AppBookingTour.Domain.Entities;

public class Hotel : BaseEntity
{
    public int CityId { get; set; }
    public int? TypeHotel { get; set; }
    public string Name { get; set; } = null!;
    public string? Address { get; set; }
    public int StarRating { get; set; }
    public decimal? Rating { get; set; }
    public string? ImageGallery { get; set; } // JSON
    public string? Description { get; set; } // Rich text
    public string? Regulation { get; set; } // Rich text
    public string? Amenities { get; set; } // JSON
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual City City { get; set; } = null!;
    public virtual ICollection<RoomType> RoomTypes { get; set; } = [];
    public virtual ICollection<Review> Reviews { get; set; } = [];
}