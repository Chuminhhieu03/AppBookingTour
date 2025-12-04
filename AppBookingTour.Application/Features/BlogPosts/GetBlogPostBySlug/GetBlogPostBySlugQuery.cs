using AppBookingTour.Application.Features.BlogPosts.GetBlogPostById;
using MediatR;

namespace AppBookingTour.Application.Features.BlogPosts.GetBlogPostBySlug;

public record GetBlogPostBySlugQuery(string Slug) : IRequest<BlogPostDetailDto?>;
