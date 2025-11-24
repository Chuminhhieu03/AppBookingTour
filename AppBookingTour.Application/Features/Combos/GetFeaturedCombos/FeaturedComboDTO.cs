using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Application.Features.Combos.GetFeaturedCombos;

public class FeaturedComboDTO
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int DurationDays { get; set; }
    public decimal BasePriceAdult { get; set; }
    public decimal BasePriceChildren { get; set; }
    public string FromCityName { get; set; } = "";
    public string ToCityName { get; set; } = "";
    public Vehicle Vehicle { get; set; }
    public string? ComboImageCoverUrl { get; set; }
    public decimal Rating { get; set; }
    public List<FeaturedComboScheduleItem> Schedules { get; set; } = new List<FeaturedComboScheduleItem>();
}

public class FeaturedComboScheduleItem
{
    public int Id { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public int AvailableSlots { get; set; }
}
