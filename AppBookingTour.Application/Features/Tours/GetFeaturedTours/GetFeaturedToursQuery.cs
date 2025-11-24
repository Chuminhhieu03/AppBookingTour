using MediatR;

namespace AppBookingTour.Application.Features.Tours.GetFeaturedTours;

public record GetFeaturedToursQuery(int Count) : IRequest<List<FeaturedTourDTO>>;
