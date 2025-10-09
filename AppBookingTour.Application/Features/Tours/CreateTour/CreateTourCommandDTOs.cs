namespace AppBookingTour.Application.Features.Tours.CreateTour;

public class TourRequestDTO
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int TypeId { get; set; }
    public int CategoryId { get; set; }
    public int DepartureCityId { get; set; }
    public int DurationDays { get; set; }
    public int DurationNights { get; set; }
    public int MaxParticipants { get; set; }
    public int MinParticipants { get; set; }
    public decimal BasePriceAdult { get; set; }
    public decimal BasePriceChild { get; set; }
    public int Status { get; set; }
    public bool IsActive { get; set; }
}