namespace AppBookingTour.Application.Features.Combos.DeleteCombo;

public class DeleteComboResponse
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }

    public static DeleteComboResponse Success() =>
        new() { IsSuccess = true };

    public static DeleteComboResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, ErrorMessage = errorMessage };
}