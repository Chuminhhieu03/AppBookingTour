using MediatR;

namespace AppBookingTour.Application.Features.BlogPosts.GetBlogPostById;

public record GetBlogPostByIdQuery(int Id) : IRequest<BlogPostDetailDto?>;
