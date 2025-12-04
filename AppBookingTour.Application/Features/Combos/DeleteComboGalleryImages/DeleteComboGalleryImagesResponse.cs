namespace AppBookingTour.Application.Features.Combos.DeleteComboGalleryImages;

public sealed class DeleteComboGalleryImagesResponse
{
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public int DeletedCount { get; set; }

    public static DeleteComboGalleryImagesResponse Success(int deletedCount) => new()
    {
        IsSuccess = true,
        DeletedCount = deletedCount
    };

    public static DeleteComboGalleryImagesResponse Failed(string errorMessage) => new()
    {
        IsSuccess = false,
        ErrorMessage = errorMessage
    };
}
