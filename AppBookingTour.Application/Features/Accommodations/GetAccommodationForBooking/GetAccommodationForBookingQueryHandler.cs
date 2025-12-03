using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Accommodations.GetAccommodationForBooking;

/// <summary>
/// Handler for GetAccommodationForBooking - Get accommodation with selected room inventories
/// </summary>
public class GetAccommodationForBookingQueryHandler : IRequestHandler<GetAccommodationForBookingQuery, AccommodationForBookingDTO?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetAccommodationForBookingQueryHandler> _logger;

    public GetAccommodationForBookingQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetAccommodationForBookingQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<AccommodationForBookingDTO?> Handle(
        GetAccommodationForBookingQuery request, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Getting accommodation for booking with {Count} room inventory IDs", 
            request.RoomInventoryIds.Count);

        // Validate input
        if (request.RoomInventoryIds == null || !request.RoomInventoryIds.Any())
        {
            _logger.LogWarning("Room inventory IDs list is empty");
            return null;
        }

        // Get all selected room inventories
        var roomInventories = await _unitOfWork.Repository<RoomInventory>()
            .FindAsync(
                ri => request.RoomInventoryIds.Contains(ri.Id),
                cancellationToken);

        var inventoryList = roomInventories.OrderBy(ri => ri.Date).ToList();

        // Validate we got all requested inventories
        if (inventoryList.Count != request.RoomInventoryIds.Count)
        {
            _logger.LogWarning(
                "Some room inventories not found. Requested: {Requested}, Found: {Found}",
                request.RoomInventoryIds.Count, inventoryList.Count);
            return null;
        }

        // Check all inventories belong to the same room type
        var roomTypeId = inventoryList.First().RoomTypeId;
        if (inventoryList.Any(ri => ri.RoomTypeId != roomTypeId))
        {
            _logger.LogWarning("Room inventories belong to different room types");
            return null;
        }

        // Get room type
        var roomType = await _unitOfWork.Repository<RoomType>()
            .GetByIdAsync(roomTypeId, cancellationToken);

        if (roomType == null)
        {
            _logger.LogWarning("RoomType not found: {RoomTypeId}", roomTypeId);
            return null;
        }

        if (roomType.Status != true)
        {
            _logger.LogWarning("RoomType is inactive: {RoomTypeId}", roomTypeId);
            return null;
        }

        // Get accommodation
        var accommodation = await _unitOfWork.Accommodations.GetById(roomType.AccommodationId);

        if (accommodation == null)
        {
            _logger.LogWarning("Accommodation not found: {AccommodationId}", roomType.AccommodationId);
            return null;
        }

        if (!accommodation.IsActive)
        {
            _logger.LogWarning("Accommodation is inactive: {AccommodationId}", accommodation.Id);
            return null;
        }

        // Calculate total price and check availability
        decimal totalPrice = 0;
        var inventoryDTOs = new List<RoomInventoryInfoDTO>();

        var availableRooms = (roomType.Quantity ?? 0) - inventoryList.Count;

        // Check if any night is fully booked
        if (availableRooms <= 0)
        {
            _logger.LogWarning("No available rooms");
            return null;
        }

        foreach (var inventory in inventoryList)
        {
            totalPrice += inventory.BasePriceAdult; // Sum all BasePriceAdult

            inventoryDTOs.Add(new RoomInventoryInfoDTO
            {
                Id = inventory.Id,
                Date = inventory.Date,
                BasePriceAdult = inventory.BasePriceAdult,
                BasePriceChildren = inventory.BasePriceChildren,
                BasePrice = inventory.BasePrice,
                AvailableRooms = availableRooms
            });
        }

        var numberOfNights = inventoryList.Count;

        _logger.LogInformation(
            "Successfully retrieved accommodation {AccommodationId}, roomType {RoomTypeId} with {Count} inventories, total price: {TotalPrice}",
            accommodation.Id, roomType.Id, numberOfNights, totalPrice);

        return new AccommodationForBookingDTO
        {
            Accommodation = new AccommodationInfoDTO
            {
                Id = accommodation.Id,
                Code = accommodation.Code,
                Name = accommodation.Name,
                Address = accommodation.Address,
                CoverImgUrl = accommodation.CoverImgUrl,
                StarRating = accommodation.StarRating,
                Rating = accommodation.Rating ?? 0,
                CityName = accommodation.City?.Name,
                Description = accommodation.Description
            },
            RoomType = new RoomTypeInfoDTO
            {
                Id = roomType.Id,
                Name = roomType.Name,
                Price = roomType.Price,
                MaxAdult = roomType.MaxAdult,       // For booking form
                MaxChildren = roomType.MaxChildren, // For booking form
                Quantity = roomType.Quantity,
                CoverImageUrl = roomType.CoverImageUrl
            },
            RoomInventories = inventoryDTOs,
            TotalPrice = totalPrice,
            NumberOfNights = numberOfNights
        };
    }
}
