
namespace AppBookingTour.Application.Features.Combos.UpdateCombo;

public class UpdateComboResponse
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }

    public static UpdateComboResponse Success() =>
        new() { IsSuccess = true };

    public static UpdateComboResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, ErrorMessage = errorMessage };
}