using Microsoft.AspNetCore.Http;

namespace AppBookingTour.Application.Features.Combos.UploadComboImages;

public class UploadComboImagesResponse
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public string? CoverImageUrl { get; init; }
    public List<string> ImageUrls { get; init; } = [];

    public static UploadComboImagesResponse Success(string? coverUrl, List<string> imageUrls) =>
        new() { IsSuccess = true, CoverImageUrl = coverUrl, ImageUrls = imageUrls };

    public static UploadComboImagesResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, ErrorMessage = errorMessage };
}
