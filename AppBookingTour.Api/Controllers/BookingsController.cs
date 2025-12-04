using AppBookingTour.Api.Contracts.Responses;
using AppBookingTour.Application.Features.Bookings.ApplyDiscount;
using AppBookingTour.Application.Features.Bookings.CreateBooking;
using AppBookingTour.Application.Features.Bookings.GetBookingById;
using AppBookingTour.Application.Features.Bookings.GetPaymentStatus;
using AppBookingTour.Application.Features.Bookings.GetUserBookings;
using AppBookingTour.Application.Features.Bookings.InitiatePayment;
using AppBookingTour.Application.Features.Bookings.PaymentCallback;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppBookingTour.Api.Controllers;

[ApiController]
[Route("api/bookings")]
public class BookingsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<BookingsController> _logger;

    public BookingsController(IMediator mediator, ILogger<BookingsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// T?o booking m?i
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<object>>> CreateBooking(
        [FromBody] CreateBookingRequestDTO request
    )
    {
        var command = new CreateBookingCommand(request);
        var result = await _mediator.Send(command);

        _logger.LogInformation("Booking created successfully: {BookingCode}", result.BookingCode);
        return Created(
            $"/api/bookings/{result.Id}",
            ApiResponse<object>.Ok(result, "T?o booking thÃ nh cÃ´ng")
        );
    }

    /// <summary>
    /// �p d?ng m� gi?m gi�
    /// </summary>
    [HttpPost("apply-discount")]
    [Obsolete("This endpoint is deprecated. Please use POST /api/discounts/apply instead.")]
    public async Task<ActionResult<ApiResponse<object>>> ApplyDiscount(
        [FromBody] ApplyDiscountRequestDTO request
    )
    {
        var command = new ApplyDiscountCommand(request);
        var result = await _mediator.Send(command);

        return Ok(ApiResponse<object>.Ok(result));
    }

    /// <summary>
    /// L?y thÃ´ng tin chi ti?t booking
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<object>>> GetBookingById(int id)
    {
        var query = new GetBookingByIdQuery(id);
        var result = await _mediator.Send(query);

        return Ok(ApiResponse<object>.Ok(result));
    }

    /// <summary>
    /// Kh?i t?o thanh toÃ¡n
    /// </summary>
    [HttpPost("payment")]
    public async Task<ActionResult<ApiResponse<object>>> InitiatePayment(
        [FromBody] InitiatePaymentRequestDTO request
    )
    {
        try
        {
            // Get IP address
            request.IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";

            var command = new InitiatePaymentCommand(request);
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(ApiResponse<object>.Fail(result.Message));
            }

            _logger.LogInformation(
                "Payment initiated successfully: {PaymentNumber}",
                result.PaymentNumber
            );
            return Ok(ApiResponse<object>.Ok(result, result.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initiating payment");
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
    }

    /// <summary>
    /// Callback t? VNPay sau khi thanh toÃ¡n
    /// </summary>
    [HttpGet("payment-callback")]
    public async Task<IActionResult> PaymentCallback()
    {
        var vnpayData = Request.Query.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());

        var command = new PaymentCallbackCommand(
            new PaymentCallbackRequestDTO { VnPayData = vnpayData }
        );

        var result = await _mediator.Send(command);

        // Redirect to FE with result
        var returnUrl = result.RedirectUrl;

        return Redirect(returnUrl);
    }

    /// <summary>
    /// Ki?m tra tr?ng thÃ¡i thanh toÃ¡n
    /// </summary>
    [HttpGet("payment-status/{bookingId:int}")]
    public async Task<ActionResult<ApiResponse<object>>> GetPaymentStatus(int bookingId)
    {
        try
        {
            var query = new GetPaymentStatusQuery(bookingId);
            var result = await _mediator.Send(query);

            return Ok(ApiResponse<object>.Ok(result));
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Booking not found: {BookingId}", bookingId);
            return NotFound(ApiResponse<object>.Fail(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting payment status for booking {BookingId}", bookingId);
            return BadRequest(ApiResponse<object>.Fail(ex.Message));
        }
    }

    /// <summary>
    /// Get bookings for a specific user with filtering and pagination
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="pageIndex">Page index (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10, max: 50)</param>
    /// <param name="status">Filter by booking status (optional)</param>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<ApiResponse<GetUserBookingsResponse>>> GetUserBookings(
        int userId,
        [FromQuery] int? pageIndex,
        [FromQuery] int? pageSize,
        [FromQuery] Domain.Enums.BookingStatus? status)
    {
        var effectivePageSize = pageSize ?? 10;
        if (effectivePageSize <= 0 || effectivePageSize > 50)
        {
            return BadRequest(ApiResponse<GetUserBookingsResponse>.Fail("Page size phải từ 1 đến 50"));
        }

        var filter = new GetUserBookingsFilter
        {
            Status = status
        };

        var query = new GetUserBookingsQuery(userId, pageIndex, effectivePageSize, filter);
        var result = await _mediator.Send(query);

        _logger.LogInformation("Retrieved {Count} bookings for user {UserId}", result.Bookings.Count, userId);
        return Ok(ApiResponse<GetUserBookingsResponse>.Ok(result));
    }
}
