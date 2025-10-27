using AppBookingTour.Application.Features.BlogPosts.GetBlogPostById;
using MediatR;

namespace AppBookingTour.Application.Features.BlogPosts.GetListBlogPosts;

public record GetBlogPostBySlugQuery(string Slug) : IRequest<BlogPostDetailDto?>;
