
using AppBookingTour.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace AppBookingTour.Application.Features.Profiles.UpdateProfile;

public class UpdateProfileDTO
{
    public string? FullName { get; set; }
    public Gender? Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public IFormFile? ProfileImageFile { get; set; }
    public string? Address { get; set; }
}
