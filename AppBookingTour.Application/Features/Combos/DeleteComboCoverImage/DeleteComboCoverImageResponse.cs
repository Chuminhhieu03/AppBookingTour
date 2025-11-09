namespace AppBookingTour.Application.Features.Combos.DeleteComboCoverImage;

public sealed class DeleteComboCoverImageResponse
{
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }

    public static DeleteComboCoverImageResponse Success() => new() { IsSuccess = true };

    public static DeleteComboCoverImageResponse Failed(string errorMessage) => new()
    {
        IsSuccess = false,
        ErrorMessage = errorMessage
    };
}
