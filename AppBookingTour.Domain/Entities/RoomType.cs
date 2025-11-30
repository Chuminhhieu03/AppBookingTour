using AppBookingTour.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppBookingTour.Domain.Entities;

public class RoomType : BaseEntity
{
    #region Primary props

    public int AccommodationId { get; set; }
    public string Name { get; set; }
    public int? MaxAdult { get; set; }
    public int? MaxChildren { get; set; }
    public bool? Status { get; set; }
    public decimal? Price { get; set; }
    public int? Quantity { get; set; }
    public string? Amenities { get; set; } // List id: 1, 2, 3, ..
    [Precision(12, 2)]
    public string? CoverImageUrl { get; set; }
    public decimal ExtraAdultPrice { get; set; }
    public decimal ExtraChildrenPrice { get; set; }
    public TimeOnly CheckinHour { get; set; }
    public TimeOnly CheckoutHour { get; set; }
    public decimal Area { get; set; }
    public string? View { get; set; } // View biển, view đường phố ... List Id
    public string? CancelPolicy { get; set; }

    #endregion

    #region Extend props
    [NotMapped]
    public string? StatusName { get; set; }
    [NotMapped]
    public string? StatusColor { get; set; }
    [NotMapped]
    public string? AmenityName { get; set; }
    [NotMapped]
    public List<Image>? ListInfoImage { get; set; }

    // Navigation properties
    public virtual ICollection<RoomInventory> ListRoomInventory { get; set; } = [];

    #endregion
}
