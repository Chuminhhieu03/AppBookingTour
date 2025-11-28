using MediatR;

namespace AppBookingTour.Application.Features.Reviews.CreateReview;

public record CreateReviewCommand(CreateReviewRequest Request) : IRequest<CreateReviewResponse>;
