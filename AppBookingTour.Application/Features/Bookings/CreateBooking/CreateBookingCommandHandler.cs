using AppBookingTour.Application.Features.Bookings.Common;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.Enums;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AppBookingTour.Application.Features.Bookings.CreateBooking;

public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, BookingDetailsDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateBookingCommandHandler> _logger;

    public CreateBookingCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CreateBookingCommandHandler> logger
    )
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<BookingDetailsDTO> Handle(
        CreateBookingCommand request,
        CancellationToken cancellationToken
    )
    {
        var req = request.Request;
        _logger.LogInformation(
            "Creating booking for user {UserId}, ItemId {ItemId}, Type {BookingType}",
            req.UserId,
            req.ItemId,
            req.BookingType
        );

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            // 1. Validate tour/combo/accommodation and get pricing information
            Tour? tour = null;
            TourDeparture? tourDeparture = null;
            Combo? combo = null;
            ComboSchedule? comboSchedule = null;
            Accommodation? accommodation = null;
            List<RoomInventory>? roomInventories = null;
            string? itemName = null;
            string? itemImageUrl = null;
            DateTime? departureDate = null;
            DateTime? returnDate = null;

            // Variables to store pricing from schedule
            decimal priceAdult = 0;
            decimal priceChild = 0;
            decimal singleRoomSurcharge = 0;
            decimal totalRoomPrice = 0;

            if (req.BookingType == BookingType.Tour)
            {
                tour = await _unitOfWork.Tours.GetByIdAsync(req.ItemId, cancellationToken);
                if (tour == null)
                    throw new ArgumentException("Tour không tồn tại");

                itemName = tour.Name;
                itemImageUrl = tour.ImageMainUrl;

                if (req.TourDepartureId.HasValue)
                {
                    tourDeparture = await _unitOfWork.TourDepartures.GetByIdAsync(
                        req.TourDepartureId.Value,
                        cancellationToken
                    );
                    if (tourDeparture == null || tourDeparture.TourId != req.ItemId)
                        throw new ArgumentException("Lịch khởi hành không hợp lệ");

                    var totalPeople = req.NumAdults + req.NumChildren;
                    if (tourDeparture.AvailableSlots < totalPeople)
                        throw new InvalidOperationException("Không đủ chỗ trống cho tour này");

                    departureDate = tourDeparture.DepartureDate;
                    returnDate = tourDeparture.ReturnDate;

                    // Get pricing from TourDeparture
                    priceAdult = tourDeparture.PriceAdult;
                    priceChild = tourDeparture.PriceChildren;
                    singleRoomSurcharge = tourDeparture.SingleRoomSurcharge;
                }
                else
                {
                    priceAdult = tour.BasePriceAdult;
                    priceChild = tour.BasePriceChild;
                    singleRoomSurcharge = 0;
                }
            }
            else if (req.BookingType == BookingType.Combo)
            {
                combo = await _unitOfWork.Combos.GetByIdAsync(req.ItemId, cancellationToken);
                if (combo == null)
                    throw new ArgumentException("Combo không tồn tại");

                itemName = combo.Name;
                itemImageUrl = combo.ComboImageCoverUrl;

                if (req.TourDepartureId.HasValue)
                {
                    comboSchedule = await _unitOfWork
                        .Repository<ComboSchedule>()
                        .GetByIdAsync(req.TourDepartureId.Value, cancellationToken);

                    if (comboSchedule == null || comboSchedule.ComboId != req.ItemId)
                        throw new ArgumentException("Lịch khởi hành combo không hợp lệ");

                    var totalPeople = req.NumAdults + req.NumChildren;
                    if (comboSchedule.AvailableSlots < totalPeople)
                        throw new InvalidOperationException("Không đủ chỗ trống cho combo này");

                    departureDate = comboSchedule.DepartureDate;
                    returnDate = comboSchedule.ReturnDate;

                    // Get pricing from ComboSchedule - Auto-fetch SingleRoomSupplement
                    priceAdult = comboSchedule.BasePriceAdult;
                    priceChild = comboSchedule.BasePriceChildren;
                    singleRoomSurcharge = comboSchedule.SingleRoomSupplement;

                    _logger.LogInformation(
                        "Combo pricing - Adult: {PriceAdult}, Child: {PriceChild}, SingleRoom: {SingleRoom}",
                        priceAdult,
                        priceChild,
                        singleRoomSurcharge
                    );
                }
                else
                {
                    priceAdult = combo.BasePriceAdult;
                    priceChild = combo.BasePriceChildren;
                    singleRoomSurcharge = 0;
                }
            }
            else if (req.BookingType == BookingType.Accommodation)
            {
                // Validate RoomInventoryIds
                if (req.RoomInventoryIds == null || !req.RoomInventoryIds.Any())
                    throw new ArgumentException(
                        "RoomInventoryIds không được để trống cho Accommodation"
                    );

                // Get all room inventories
                roomInventories = (
                    await _unitOfWork
                        .Repository<RoomInventory>()
                        .FindAsync(ri => req.RoomInventoryIds.Contains(ri.Id), cancellationToken)
                ).ToList();

                if (roomInventories.Count != req.RoomInventoryIds.Count)
                    throw new ArgumentException("Một số RoomInventory không tồn tại");

                // Check all belong to same room type
                var roomTypeId = roomInventories.First().RoomTypeId;
                if (roomInventories.Any(ri => ri.RoomTypeId != roomTypeId))
                    throw new ArgumentException("Tất cả RoomInventory phải cùng một RoomType");

                // Get room type
                var roomType = await _unitOfWork
                    .Repository<RoomType>()
                    .GetByIdAsync(roomTypeId, cancellationToken);

                if (roomType == null)
                    throw new ArgumentException("RoomType không tồn tại");

                // Get accommodation
                accommodation = await _unitOfWork.Accommodations.GetById(roomType.AccommodationId);

                if (accommodation == null)
                    throw new ArgumentException("Accommodation không tồn tại");

                if (!accommodation.IsActive)
                    throw new ArgumentException("Accommodation không còn hoạt động");

                // Check availability
                foreach (var inventory in roomInventories)
                {
                    var availableRooms = inventory.BookedRooms - 1;
                    if (availableRooms < 0)
                        throw new InvalidOperationException(
                            $"Không còn phòng trống cho ngày {inventory.Date:dd/MM/yyyy}"
                        );
                }

                itemName = accommodation.Name;
                itemImageUrl = accommodation.CoverImgUrl;
                departureDate = roomInventories.Min(ri => ri.Date);
                returnDate = roomInventories.Max(ri => ri.Date);
                totalRoomPrice = roomInventories.Sum(x => x.BasePrice);
            }
            else
            {
                throw new ArgumentException($"BookingType {req.BookingType} chưa được hỗ trợ");
            }

            // 2. Calculate pricing using values from schedule

            decimal totalAmount = 0m;

            if (req.BookingType == BookingType.Accommodation)
            {
                totalAmount = totalRoomPrice;
            }
            else
            {
                totalAmount =
                    (req.NumAdults * priceAdult)
                    + (req.NumChildren * priceChild)
                    + (req.NumSingleRooms * singleRoomSurcharge);
            }

            _logger.LogInformation(
                "Calculated total: {Total} = Adults({NumAdults}x{PriceAdult}) + Children({NumChildren}x{PriceChild}) + SingleRooms({NumSingle}x{SingleSurcharge})",
                totalAmount,
                req.NumAdults,
                priceAdult,
                req.NumChildren,
                priceChild,
                req.NumSingleRooms,
                singleRoomSurcharge
            );

            decimal discountAmount = 0;
            Discount? discount = null;

            // 3. Apply discount
            if (!string.IsNullOrEmpty(req.DiscountCode))
            {
                discount = await _unitOfWork.Discounts.FirstOrDefaultAsync(
                    d => d.Code == req.DiscountCode && d.Status == 1,
                    cancellationToken
                );

                if (discount != null)
                {
                    var validationResult = ValidateDiscount(discount, totalAmount);
                    if (validationResult.IsValid)
                        discountAmount = CalculateDiscount(discount, totalAmount);
                    else
                        throw new InvalidOperationException(validationResult.ErrorMessage);
                }
            }

            var finalAmount = totalAmount - discountAmount;
            var bookingCode = await GenerateUniqueBookingCode(cancellationToken);

            // 4. Create booking
            var booking = new Booking
            {
                BookingCode = bookingCode,
                UserId = req.UserId,
                BookingType = req.BookingType,
                ItemId = req.ItemId,
                BookingDate = DateTime.UtcNow,
                TravelDate = req.TravelDate,
                NumAdults = req.NumAdults,
                NumChildren = req.NumChildren,
                NumSingleRooms = req.NumSingleRooms,
                AdultPrice = priceAdult,
                ChildPrice = priceChild,
                SingleRoomPrice = singleRoomSurcharge,
                TotalAmount = totalAmount,
                DiscountAmount = discountAmount,
                FinalAmount = finalAmount,
                PaymentType = req.PaymentType,
                Status = BookingStatus.Pending,
                SpecialRequests = req.SpecialRequests,
                ContactName = req.ContactName,
                ContactPhone = req.ContactPhone,
                ContactEmail = req.ContactEmail,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await _unitOfWork.Bookings.AddAsync(booking, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 5. Create participants
            var participants = req
                .Participants.Select(p => new BookingParticipant
                {
                    BookingId = booking.Id,
                    FullName = p.FullName,
                    DateOfBirth = p.DateOfBirth,
                    Gender = p.Gender,
                    IdNumber = p.IdNumber,
                    Nationality = p.Nationality,
                    NeedSingleRoom = p.NeedSingleRoom,
                    ParticipantType = p.ParticipantType,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                })
                .ToList();

            await _unitOfWork.BookingParticipants.AddRangeAsync(participants, cancellationToken);

            // 6. Record discount usage
            if (discount != null && discountAmount > 0)
            {
                var discountUsage = new DiscountUsage
                {
                    DiscountId = discount.Id,
                    UserId = req.UserId,
                    BookingId = booking.Id,
                    DiscountAmount = discountAmount,
                    UsedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                await _unitOfWork.DiscountUsages.AddAsync(discountUsage, cancellationToken);

                if (discount.RemainQuantity.HasValue)
                {
                    discount.RemainQuantity--;
                    _unitOfWork.Discounts.Update(discount);
                }
            }

            // 7. Update slots
            if (tourDeparture != null)
            {
                var totalPeople = req.NumAdults + req.NumChildren;
                tourDeparture.BookedSlots += totalPeople;
                tourDeparture.AvailableSlots -= totalPeople;
                if (tourDeparture.AvailableSlots == 0)
                    tourDeparture.Status = DepartureStatus.Full;
                else if (tourDeparture.AvailableSlots < 0)
                    throw new InvalidOperationException(
                        "Số lượng người tham gia tour đã quá số lượng cho phép, xin bạn vui lòng đổi sang ngày khác"
                    );
                _unitOfWork.TourDepartures.Update(tourDeparture);
            }
            else if (comboSchedule != null)
            {
                var totalPeople = req.NumAdults + req.NumChildren;
                comboSchedule.BookedSlots += totalPeople;
                comboSchedule.AvailableSlots -= totalPeople;
                if (comboSchedule.AvailableSlots == 0)
                    comboSchedule.Status = ComboStatus.Full;
                else if (comboSchedule.AvailableSlots < 0)
                    throw new InvalidOperationException(
                        "Số lượng người tham gia combo đã quá số lượng cho phép, xin bạn vui lòng đổi sang ngày khác"
                    );
                _unitOfWork.Repository<ComboSchedule>().Update(comboSchedule);
            }
            else if (roomInventories != null && roomInventories.Any())
            {
                // Update booked rooms for each inventory
                foreach (var inventory in roomInventories)
                {
                    inventory.BookedRooms += 1; // FIX: Phải cộng, không phải trừ
                    _unitOfWork.Repository<RoomInventory>().Update(inventory);
                }

                // ⭐ LƯU CHI TIẾT GIÁ TỪNG ĐÊM
                var roomDetails = roomInventories.Select(ri => new BookingRoomDetail
                {
                    BookingId = booking.Id,
                    RoomInventoryId = ri.Id,
                    Date = ri.Date,
                    BasePriceAdult = ri.BasePriceAdult,
                    BasePriceChildren = ri.BasePriceChildren,
                    BasePrice = ri.BasePrice,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }).ToList();

                await _unitOfWork.Repository<BookingRoomDetail>()
                    .AddRangeAsync(roomDetails, cancellationToken);

                _logger.LogInformation(
                    "Updated {Count} room inventories and saved room details for accommodation booking",
                    roomInventories.Count
                );
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            _logger.LogInformation("Booking created successfully: {BookingCode}", bookingCode);

            // 8. Return booking details
            return new BookingDetailsDTO
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
                NumSingleRooms = booking.NumSingleRooms,
                TotalAmount = booking.TotalAmount,
                AdultPrice = booking.AdultPrice,
                ChildPrice = booking.ChildPrice,
                InfantPrice = 0, // TODO: Fix lại cho hợp với mọi loại booking
                SingleRoomPrice = booking.SingleRoomPrice,
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
                PaidAmount = 0,
                RemainingAmount = booking.FinalAmount,
                TourName = itemName,
                TourImageUrl = itemImageUrl,
                DepartureDate = departureDate,
                ReturnDate = returnDate,
                Participants = _mapper.Map<List<BookingParticipantDTO>>(participants),
                DiscountCode = discount?.Code,
                DiscountName = discount?.Name,
            };
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    private async Task<string> GenerateUniqueBookingCode(CancellationToken cancellationToken)
    {
        string code;
        bool exists;
        do
        {
            code = $"BK{DateTime.UtcNow:yyyyMMdd}{Random.Shared.Next(1000, 9999)}";
            exists = await _unitOfWork.Bookings.IsBookingCodeExistsAsync(code, cancellationToken);
        } while (exists);
        return code;
    }

    private (bool IsValid, string? ErrorMessage) ValidateDiscount(
        Discount discount,
        decimal totalAmount
    )
    {
        var now = DateTime.UtcNow;

        if (discount.StartEffectedDtg.HasValue && now < discount.StartEffectedDtg.Value)
            return (false, "Mã giảm giá chưa có hiệu lực");

        if (discount.EndEffectedDtg.HasValue && now > discount.EndEffectedDtg.Value)
            return (false, "Mã giảm giá đã hết hạn");

        if (discount.RemainQuantity.HasValue && discount.RemainQuantity.Value <= 0)
            return (false, "Mã giảm giá đã hết lượt sử dụng");

        if (discount.MinimumOrderAmount.HasValue && totalAmount < discount.MinimumOrderAmount.Value)
            return (
                false,
                $"Đơn hàng tối thiểu {discount.MinimumOrderAmount.Value:N0} VNĐ để áp dụng mã này"
            );

        return (true, null);
    }

    private decimal CalculateDiscount(Discount discount, decimal totalAmount)
    {
        if (!discount.DiscountPercent.HasValue)
            return 0;

        var discountAmount = totalAmount * (discount.DiscountPercent.Value / 100);

        if (discount.MaximumDiscount.HasValue && discountAmount > discount.MaximumDiscount.Value)
            discountAmount = discount.MaximumDiscount.Value;

        return Math.Round(discountAmount, 0);
    }
}
