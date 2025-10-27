using AppBookingTour.Domain.Enums;
using AppBookingTour.Share.DTOS;
using MediatR;

namespace AppBookingTour.Application.Features.BlogPosts.UpdateBlogPost;

public record UpdateBlogPostCommand(UpdateBlogPostRequest Request) : IRequest<UpdateBlogPostResponse>;
