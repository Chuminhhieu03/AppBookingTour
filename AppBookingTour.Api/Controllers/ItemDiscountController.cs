using AppBookingTour.Api.Contracts.Responses;
using AppBookingTour.Application.Features.ItemDiscounts.AssignDiscount;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppBookingTour.Api.Controllers;

[ApiController]
[Route("api/item-discounts")]
public class ItemDiscountController : ControllerBase
{
    private readonly IMediator _mediator;

    public ItemDiscountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("assign-discount")]
    public async Task<ActionResult<ApiResponse<AssignDiscountResponse>>> AssignDiscount(
        [FromBody] AssignDiscountRequestDTO request)
    {
        var command = new AssignDiscountCommand(request);
        var result = await _mediator.Send(command);
        if (!result.Success)
        {
            return BadRequest(ApiResponse<AssignDiscountResponse>.Fail(result.Message));
        }
        return Ok(ApiResponse<AssignDiscountResponse>.Ok(result));
    }
}

