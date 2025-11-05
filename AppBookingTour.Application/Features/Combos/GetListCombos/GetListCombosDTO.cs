namespace AppBookingTour.Application.Features.Combos.GetListCombos;

public class GetListCombosRequest
{
    public int PageIndex { get; set; } = 0;
    public int PageSize { get; set; } = 20;
    public int? FromCityId { get; set; }
    public int? ToCityId { get; set; }
    public int? Vehicle { get; set; }
    public bool? IsActive { get; set; } = true;
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? MinDuration { get; set; }
    public int? MaxDuration { get; set; }
    public string? SearchTerm { get; set; }
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; } = false;
}

public class ComboListDTO
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int FromCityId { get; set; }
    public string FromCityName { get; set; } = null!;
    public int ToCityId { get; set; }
    public string ToCityName { get; set; } = null!;
    public string? ShortDescription { get; set; }
    public int Vehicle { get; set; }
    public int DurationDays { get; set; }
    public decimal BasePriceAdult { get; set; }
    public decimal BasePriceChildren { get; set; }
    public string? ComboImageCoverUrl { get; set; }
    public decimal Rating { get; set; }
    public int TotalBookings { get; set; }
    public int ViewCount { get; set; }
    public bool IsActive { get; set; }
    public int AvailableSchedulesCount { get; set; }
    public DateTime? NextDepartureDate { get; set; }
}
