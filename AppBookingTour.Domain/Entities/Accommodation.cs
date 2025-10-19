using System.ComponentModel.DataAnnotations.Schema;

namespace AppBookingTour.Domain.Entities;

public class Accommodation : BaseEntity
{
    public int CityId { get; set; }
    public int? TypeHotel { get; set; }
    public string Name { get; set; } = null!;
    public string? Address { get; set; }
    public int StarRating { get; set; }
    public decimal? Rating { get; set; }
    public string? Description { get; set; } // Rich text
    public string? Regulation { get; set; } // Rich text
    public string? Amenities { get; set; } // JSON
    public bool IsActive { get; set; } = true;
    public string? HotelImageCoverUrl { get; set; }
    [NotMapped]
    public List<Image>? Images { get; set; }

    // Navigation properties
    public virtual City City { get; set; } = null!;
    public virtual ICollection<RoomType> RoomTypes { get; set; } = [];
    public virtual ICollection<Review> Reviews { get; set; } = [];
}