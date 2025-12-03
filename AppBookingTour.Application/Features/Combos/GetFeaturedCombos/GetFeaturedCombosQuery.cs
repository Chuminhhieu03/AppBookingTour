using MediatR;

namespace AppBookingTour.Application.Features.Combos.GetFeaturedCombos;

public record GetFeaturedCombosQuery(int Count) : IRequest<List<FeaturedComboDTO>>;
