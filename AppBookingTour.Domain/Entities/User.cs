using AppBookingTour.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace AppBookingTour.Domain.Entities;

/// <summary>
/// User entity with centralized enums
/// </summary>
public class User : IdentityUser<int>
{
    public string FullName { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; }
    public Gender? Gender { get; set; } // Using centralized enum
    public string? Address { get; set; }
    public string? ProfileImage { get; set; }
    public UserType UserType { get; set; } = UserType.Customer; // Using centralized enum

    // Refresh token storage
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    // Navigation properties
    public virtual ICollection<Booking> Bookings { get; set; } = [];
    public virtual ICollection<Review> Reviews { get; set; } = [];
    public virtual ICollection<BlogPost> BlogPosts { get; set; } = [];
    public virtual ICollection<PromotionUsage> PromotionUsages { get; set; } = [];

}