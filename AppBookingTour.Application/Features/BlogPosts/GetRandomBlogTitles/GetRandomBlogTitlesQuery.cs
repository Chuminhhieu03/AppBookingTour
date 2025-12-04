using MediatR;

namespace AppBookingTour.Application.Features.BlogPosts.GetRandomBlogTitles;

public record GetRandomBlogTitlesQuery(int Count) : IRequest<List<BlogTitleDto>>;
