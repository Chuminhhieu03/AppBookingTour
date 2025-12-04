using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Application.Features.Profiles.GetListGuide;

public class GuideItemDTO
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public Gender Gender { get; set; }
    public string Email { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
}
