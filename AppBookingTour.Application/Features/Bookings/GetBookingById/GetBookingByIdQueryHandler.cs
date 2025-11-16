using AppBookingTour.Application.Features.Bookings.Common;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.Enums;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Bookings.GetBookingById;

public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, BookingDetailsDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<GetBookingByIdQueryHandler> _logger;

    public GetBookingByIdQueryHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GetBookingByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<BookingDetailsDTO> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting booking details for ID: {BookingId}", request.BookingId);

        var booking = await _unitOfWork.Bookings.GetBookingWithDetailsAsync(request.BookingId, cancellationToken);

        if (booking == null)
            throw new KeyNotFoundException($"Booking v?i ID {request.BookingId} không t?n t?i");

        // Get tour/combo information
        string? itemName = null;
        string? itemImageUrl = null;
        DateTime? departureDate = null;
        DateTime? returnDate = null;

        if (booking.BookingType == BookingType.Tour)
        {
            var tour = await _unitOfWork.Tours.GetByIdAsync(booking.ItemId, cancellationToken);
            if (tour != null)
            {
                itemName = tour.Name;
                itemImageUrl = tour.ImageMainUrl;
            }

            // Try to find tour departure info
            var tourDeparture = await _unitOfWork.TourDepartures
                .FirstOrDefaultAsync(
                    td => td.TourId == booking.ItemId && 
                          td.DepartureDate.Date == booking.TravelDate.Date,
                    cancellationToken);

            if (tourDeparture != null)
            {
                departureDate = tourDeparture.DepartureDate;
                returnDate = tourDeparture.ReturnDate;
            }
        }
        else if (booking.BookingType == BookingType.Combo)
        {
            var combo = await _unitOfWork.Combos.GetByIdAsync(booking.ItemId, cancellationToken);
            if (combo != null)
            {
                itemName = combo.Name;
                itemImageUrl = combo.ComboImageCoverUrl;
            }

            // Try to find combo schedule info
            var comboSchedule = await _unitOfWork.Repository<ComboSchedule>()
                .FirstOrDefaultAsync(
                    cs => cs.ComboId == booking.ItemId && 
                          cs.DepartureDate.Date == booking.TravelDate.Date,
                    cancellationToken);

            if (comboSchedule != null)
            {
                departureDate = comboSchedule.DepartureDate;
                returnDate = comboSchedule.ReturnDate;
            }
        }

        // Calculate paid amount
        var paidAmount = booking.Payments
            .Where(p => p.Status == PaymentStatus.Completed)
            .Sum(p => p.Amount);

        var remainingAmount = booking.FinalAmount - paidAmount;

        // Get discount info
        var discountUsage = booking.DiscountUsages.FirstOrDefault();

        var result = new BookingDetailsDTO
        {
            Id = booking.Id,
            BookingCode = booking.BookingCode,
            UserId = booking.UserId,
            BookingType = booking.BookingType,
            ItemId = booking.ItemId,
            BookingDate = booking.BookingDate,
            TravelDate = booking.TravelDate,
            NumAdults = booking.NumAdults,
            NumChildren = booking.NumChildren,
            NumInfants = booking.NumInfants,
            TotalAmount = booking.TotalAmount,
            DiscountAmount = booking.DiscountAmount,
            FinalAmount = booking.FinalAmount,
            PaymentType = booking.PaymentType,
            Status = booking.Status,
            SpecialRequests = booking.SpecialRequests,
            ContactName = booking.ContactName,
            ContactPhone = booking.ContactPhone,
            ContactEmail = booking.ContactEmail,
            CreatedAt = booking.CreatedAt,
            PaymentDeadline = booking.CreatedAt.AddMinutes(15),
            PaidAmount = paidAmount,
            RemainingAmount = remainingAmount,
            TourName = itemName,
            TourImageUrl = itemImageUrl,
            DepartureDate = departureDate,
            ReturnDate = returnDate,
            Participants = _mapper.Map<List<BookingParticipantDTO>>(booking.Participants),
            DiscountCode = discountUsage?.Discount?.Code,
            DiscountName = discountUsage?.Discount?.Name
        };

        return result;
    }
}
