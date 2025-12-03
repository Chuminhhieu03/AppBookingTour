using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Reviews.CreateReview;

public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, CreateReviewResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateReviewCommandHandler> _logger;

    public CreateReviewCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateReviewCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<CreateReviewResponse> Handle(CreateReviewCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        
        _logger.LogInformation("Creating review for booking {BookingId} by user {UserId}", 
            request.BookingId, request.UserId);

        // Get booking to retrieve ItemId and BookingType
        var booking = await _unitOfWork.Bookings.GetByIdAsync(request.BookingId, cancellationToken);
        
        if (booking == null)
        {
            _logger.LogWarning("Booking {BookingId} not found", request.BookingId);
            return new CreateReviewResponse
            {
                Success = false,
                Message = "Booking không tồn tại"
            };
        }

        // Verify user owns this booking
        if (booking.UserId != request.UserId)
        {
            _logger.LogWarning("User {UserId} does not own booking {BookingId}", request.UserId, request.BookingId);
            return new CreateReviewResponse
            {
                Success = false,
                Message = "Bạn không có quyền đánh giá booking này"
            };
        }

        // Check if booking is completed
        //if (booking.Status != BookingStatus.Completed)
        //{
        //    _logger.LogWarning("Booking {BookingId} is not completed. Status: {Status}", 
        //        request.BookingId, booking.Status);
        //    return new CreateReviewResponse
        //    {
        //        Success = false,
        //        Message = "Chỉ có thể đánh giá khi booking đã hoàn thành"
        //    };
        //}

        // Check if user already reviewed this booking
        //var existingReview = await _unitOfWork.Repository<Review>()
        //    .GetFirstOrDefaultAsync(
        //        filter: r => r.BookingId == request.BookingId && r.UserId == request.UserId,
        //        cancellationToken: cancellationToken
        //    );

        //if (existingReview != null)
        //{
        //    _logger.LogWarning("User {UserId} already reviewed booking {BookingId}", 
        //        request.UserId, request.BookingId);
        //    return new CreateReviewResponse
        //    {
        //        Success = false,
        //        Message = "Bạn đã đánh giá booking này rồi"
        //    };
        //}

        // Map BookingType to ReviewItemType
        var itemType = MapBookingTypeToReviewItemType(booking.BookingType);

        // Create review
        var review = new Review
        {
            UserId = request.UserId,
            BookingId = request.BookingId,
            ItemId = booking.ItemId,
            ItemType = itemType,
            Rating = request.Rating,
            Comment = request.Comment,
            ReviewDate = DateTime.UtcNow,
            Status = ReviewStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Repository<Review>().AddAsync(review, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Review created successfully with ID {ReviewId} for booking {BookingId}", 
            review.Id, request.BookingId);

        return new CreateReviewResponse
        {
            Success = true,
            Message = "Đánh giá thành công",
            Review = new ReviewDto
            {
                Id = review.Id,
                UserId = review.UserId,
                BookingId = review.BookingId,
                ItemId = review.ItemId,
                ItemType = review.ItemType,
                ItemTypeName = review.ItemType.ToString(),
                Rating = review.Rating,
                Comment = review.Comment,
                ReviewDate = review.ReviewDate,
                Status = review.Status,
                StatusName = review.Status.ToString()
            }
        };
    }

    private ReviewItemType MapBookingTypeToReviewItemType(BookingType bookingType)
    {
        return bookingType switch
        {
            BookingType.Tour => ReviewItemType.Tour,
            BookingType.Accommodation => ReviewItemType.Hotel,
            BookingType.Combo => ReviewItemType.Combo,
            _ => throw new ArgumentException($"Unknown booking type: {bookingType}")
        };
    }
}
