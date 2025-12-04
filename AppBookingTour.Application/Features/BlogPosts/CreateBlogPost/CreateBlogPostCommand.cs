using MediatR;

namespace AppBookingTour.Application.Features.BlogPosts.CreateBlogPost;

public record CreateBlogPostCommand(CreateBlogPostRequest Request) : IRequest<CreateBlogPostResponse>;
