namespace AppBookingTour.Application.Features.Combos.GetFeaturedCombos;

public class FeaturedComboDTO
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string FromCityName { get; set; } = null!;
    public string ToCityName { get; set; } = null!;
    public string? ShortDescription { get; set; }
    public string Vehicle { get; set; } = null!;
    public string? ComboImageCoverUrl { get; set; }
    public int DurationDays { get; set; }
    public decimal BasePriceAdult { get; set; }
    public decimal Rating { get; set; }
}
