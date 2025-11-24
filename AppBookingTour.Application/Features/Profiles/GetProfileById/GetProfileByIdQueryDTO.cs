using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Application.Features.Profiles.GetProfileById;

public class GetProfileByIdDTO
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public Gender Gender { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string ProfileImage { get; set; } = "";
    public UserType UserType { get; set; }
    public string Address { get; set; } = "";
    public string Email { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
    public int BookingCount { get; set; } 
}
