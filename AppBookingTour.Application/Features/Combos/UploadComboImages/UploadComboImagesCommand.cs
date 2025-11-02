using MediatR;
using Microsoft.AspNetCore.Http;

namespace AppBookingTour.Application.Features.Combos.UploadComboImages;

public record UploadComboImagesCommand(
    int ComboId,
    IFormFile? CoverImage,
    IFormFile[]? Images
) : IRequest<UploadComboImagesResponse>;
