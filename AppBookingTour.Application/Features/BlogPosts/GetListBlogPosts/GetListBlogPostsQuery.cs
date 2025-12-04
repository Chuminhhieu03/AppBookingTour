using AppBookingTour.Share.DTOS;
using MediatR;

namespace AppBookingTour.Application.Features.BlogPosts.GetListBlogPosts;

public record GetListBlogPostsQuery(GetListBlogPostsRequest Request) : IRequest<PagedResult<BlogPostListDto>>;
