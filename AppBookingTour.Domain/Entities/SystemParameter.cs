using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Domain.Entities;

public class SystemParameter : BaseEntity
{
    public string? Code { get; set; }
    public string Name { get; set; } = null!;
    public FeatureCode? FeatureCode { get; set; } = null;
    public string? Description;
}
