using MediatR;

namespace AppBookingTour.Application.Features.Combos.DeleteComboGalleryImages;

public sealed record DeleteComboGalleryImagesCommand(int ComboId, List<string> ImageUrls) : IRequest<DeleteComboGalleryImagesResponse>;
