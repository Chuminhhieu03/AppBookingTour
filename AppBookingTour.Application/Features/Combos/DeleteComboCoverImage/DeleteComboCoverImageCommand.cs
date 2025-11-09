using MediatR;

namespace AppBookingTour.Application.Features.Combos.DeleteComboCoverImage;

public sealed record DeleteComboCoverImageCommand(int ComboId) : IRequest<DeleteComboCoverImageResponse>;
