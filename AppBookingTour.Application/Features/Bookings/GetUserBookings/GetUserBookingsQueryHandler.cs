using AppBookingTour.Application.Features.Tours.SearchTours;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Bookings.GetUserBookings;

public class GetUserBookingsQueryHandler : IRequestHandler<GetUserBookingsQuery, GetUserBookingsResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetUserBookingsQueryHandler> _logger;

    public GetUserBookingsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetUserBookingsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<GetUserBookingsResponse> Handle(GetUserBookingsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting bookings for user {UserId}", request.UserId);

        var pageIndex = request.PageIndex ?? 1;
        var pageSize = request.PageSize ?? 10;

        // Get bookings with filter
        var (bookings, totalCount) = await _unitOfWork.Bookings.GetUserBookingsWithFilterAsync(
            request.UserId,
            request.Filter,
            pageIndex,
            pageSize,
            cancellationToken
        );

        if (!bookings.Any())
        {
            _logger.LogInformation("No bookings found for user {UserId}", request.UserId);
            return new GetUserBookingsResponse
            {
                Bookings = new List<UserBookingItem>(),
                //Meta = new PaginationMeta
                //{
                //    CurrentPage = pageIndex,
                //    PageSize = pageSize,
                //    TotalCount = 0,
                //    TotalPages = 0
                //}
            };
        }

        // Get item IDs by type
        var tourIds = bookings.Where(b => b.BookingType == BookingType.Tour).Select(b => b.ItemId).Distinct().ToList();
        var comboIds = bookings.Where(b => b.BookingType == BookingType.Combo).Select(b => b.ItemId).Distinct().ToList();
        var roomTypeIds = bookings.Where(b => b.BookingType == BookingType.Accommodation).Select(b => b.ItemId).Distinct().ToList();

        // Fetch related entities
        //var tours = tourIds.Any() 
        //    ? await _unitOfWork.Repository<Tour>().GetAllAsync(
        //        filter: t => tourIds.Contains(t.Id),
        //        includeProperties: "DepartureCity,DestinationCity",
        //        cancellationToken: cancellationToken)
        //    : new List<Tour>();

        //var combos = comboIds.Any()
        //    ? await _unitOfWork.Repository<Combo>().GetAllAsync(
        //        filter: c => comboIds.Contains(c.Id),
        //        includeProperties: "FromCity,ToCity",
        //        cancellationToken: cancellationToken)
        //    : new List<Combo>();

        //var roomTypes = roomTypeIds.Any()
        //    ? await _unitOfWork.Repository<RoomType>().GetAllAsync(
        //        filter: r => roomTypeIds.Contains(r.Id),
        //        includeProperties: "Accommodation,Accommodation.City",
        //        cancellationToken: cancellationToken)
        //    : new List<RoomType>();

        // Map bookings to DTOs
        //var bookingItems = bookings.Select(b => MapToUserBookingItem(b, tours, combos, roomTypes)).ToList();

        //var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        //_logger.LogInformation("Retrieved {Count} bookings for user {UserId}", bookingItems.Count, request.UserId);

        return new GetUserBookingsResponse
        {
            //Bookings = bookingItems,
            //Meta = new PaginationMeta
            //{
            //    CurrentPage = pageIndex,
            //    PageSize = pageSize,
            //    TotalCount = totalCount,
            //    TotalPages = totalPages
            //}
        };
    }

    private UserBookingItem MapToUserBookingItem(
        Booking booking,
        List<Tour> tours,
        List<Combo> combos,
        List<RoomType> roomTypes)
    {
        var item = new UserBookingItem
        {
            Id = booking.Id,
            BookingCode = booking.BookingCode,
            BookingType = booking.BookingType,
            BookingTypeName = booking.BookingType.ToString(),
            BookingDate = booking.BookingDate,
            TravelDate = booking.TravelDate,
            NumAdults = booking.NumAdults,
            NumChildren = booking.NumChildren,
            NumInfants = booking.NumInfants,
            FinalAmount = booking.FinalAmount,
            Status = booking.Status,
            StatusName = booking.Status.ToString(),
            ContactName = booking.ContactName,
            ContactPhone = booking.ContactPhone
        };

        // Map item details based on booking type
        switch (booking.BookingType)
        {
            case BookingType.Tour:
                var tour = tours.FirstOrDefault(t => t.Id == booking.ItemId);
                if (tour != null)
                {
                    item.ItemDetails = new BookingItemDetails
                    {
                        ItemId = tour.Id,
                        ItemCode = tour.Code,
                        ItemName = tour.Name,
                        ItemImageUrl = tour.ImageMainUrl,
                        DurationDays = tour.DurationDays,
                        DurationNights = tour.DurationNights,
                        DepartureCityName = tour.DepartureCity?.Name,
                        DestinationCityName = tour.DestinationCity?.Name
                    };
                }
                break;

            case BookingType.Combo:
                var combo = combos.FirstOrDefault(c => c.Id == booking.ItemId);
                if (combo != null)
                {
                    item.ItemDetails = new BookingItemDetails
                    {
                        ItemId = combo.Id,
                        ItemCode = combo.Code,
                        ItemName = combo.Name,
                        ItemImageUrl = combo.ComboImageCoverUrl,
                        DurationDays = combo.DurationDays,
                        FromCityName = combo.FromCity?.Name,
                        ToCityName = combo.ToCity?.Name,
                        Vehicle = combo.Vehicle.ToString()
                    };
                }
                break;

            case BookingType.Accommodation:
                var roomType = roomTypes.FirstOrDefault(r => r.Id == booking.ItemId);
                //if (roomType != null)
                //{
                //    item.ItemDetails = new BookingItemDetails
                //    {
                //        ItemId = roomType.Id,
                //        ItemCode = roomType.Accommodation?.Code,
                //        ItemName = $"{roomType.Accommodation?.Name} - {roomType.Name}",
                //        ItemImageUrl = roomType.Accommodation?.CoverImageUrl,
                //        AccommodationCityName = roomType.Accommodation?.City?.Name,
                //        AccommodationType = roomType.Accommodation?.Type.ToString()
                //    };
                //}
                break;
        }

        return item;
    }
}
