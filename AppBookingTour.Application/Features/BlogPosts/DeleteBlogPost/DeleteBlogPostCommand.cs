using AppBookingTour.Share.DTOS;
using MediatR;

namespace AppBookingTour.Application.Features.BlogPosts.DeleteBlogPost;

public record DeleteBlogPostCommand(int Id) : IRequest<DeleteBlogPostResponse>;
