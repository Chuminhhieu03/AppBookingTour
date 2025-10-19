using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.Enums;

public class Image : BaseEntity
{
    public string Url { get; set; } = null!;
    public EntityType EntityType { get; set; }
    public int? EntityId { get; set; }
    public int? Order { get; set; }
}