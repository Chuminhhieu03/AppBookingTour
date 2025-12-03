using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Application.Features.Combos.GetComboForBooking;

/// <summary>
/// Response DTO for GetComboForBooking - Contains combo parent info and selected schedule
/// </summary>
public class ComboForBookingDTO
{
    // Combo parent information (for summary card)
    public ComboInfoDTO Combo { get; set; } = null!;
    
    // Selected schedule information (for booking form)
    public ComboScheduleInfoDTO Schedule { get; set; } = null!;
}

/// <summary>
/// Combo parent information
/// </summary>
public class ComboInfoDTO
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public string? ComboImageCoverUrl { get; set; }
    public int DurationDays { get; set; }
    public string Vehicle { get; set; } = null!;
    public decimal Rating { get; set; }
    public int TotalBookings { get; set; }
    public string FromCityName { get; set; } = null!;
    public string ToCityName { get; set; } = null!;
}

/// <summary>
/// Selected combo schedule information
/// </summary>
public class ComboScheduleInfoDTO
{
    public int Id { get; set; }
    public DateTime DepartureDate { get; set; }
    public DateTime ReturnDate { get; set; }
    public decimal BasePriceAdult { get; set; }
    public decimal BasePriceChildren { get; set; }
    public decimal SingleRoomSupplement { get; set; }
    public int AvailableSlots { get; set; }
    public int BookedSlots { get; set; }
    public string Status { get; set; } = null!;
}
