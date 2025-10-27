using System.ComponentModel.DataAnnotations.Schema;

namespace AppBookingTour.Domain.Entities;

public class Accommodation : BaseEntity
{
    #region Primary props

    public int CityId { get; set; }
    public int? Type { get; set; }
    public string? Code { get; set; }
    public string Name { get; set; } = null!;
    public string? Address { get; set; }
    public int StarRating { get; set; }
    public decimal? Rating { get; set; }
    public string? Description { get; set; } // Rich text
    public string? Regulation { get; set; } // Rich text
    public string? Amenities { get; set; } // JSON
    public bool IsActive { get; set; } = true;
    public string? CoverImgUrl { get; set; }

    #endregion

    #region Extends props

    [NotMapped]
    public string? StatusName { get; set; }
    [NotMapped]
    public string? TypeName { get; set; }
    [NotMapped]
    public string? AmenityName { get; set; }
    [NotMapped]
    public List<Image>? ListInfoImage { get; set; }

    // Navigation properties
    public virtual City? City { get; set; }
    public virtual ICollection<RoomType>? ListRoomType { get; set; }
    public virtual ICollection<Review> Reviews { get; set; } = [];

    #endregion
}