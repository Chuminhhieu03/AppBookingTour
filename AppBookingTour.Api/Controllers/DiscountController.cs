using AppBookingTour.Api.Contracts.Responses;
using AppBookingTour.Application.Features.Bookings.ApplyDiscount;
using AppBookingTour.Application.Features.Discounts.AddNewDiscount;
using AppBookingTour.Application.Features.Discounts.DeleteDiscount;
using AppBookingTour.Application.Features.Discounts.GetDiscountsByEntityType;
using AppBookingTour.Application.Features.Discounts.SearchDiscounts;
using AppBookingTour.Application.Features.Discounts.SetupDiscountAddnew;
using AppBookingTour.Application.Features.Discounts.SetupDiscountDefault;
using AppBookingTour.Application.Features.Discounts.SetupDiscountDisplay;
using AppBookingTour.Application.Features.Discounts.SetupDiscountEdit;
using AppBookingTour.Application.Features.Discounts.UpdateDiscount;
using IdentityModel.OidcClient;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppBookingTour.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DiscountController> _logger;

        public DiscountController(IMediator mediator, ILogger<DiscountController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Apply discount code (New endpoint - recommended)
        /// </summary>
        [HttpPost("apply")]
        public async Task<ActionResult<ApiResponse<ApplyDiscountResponseDTO>>> ApplyDiscount(
            [FromBody] ApplyDiscountRequestDTO request)
        {
            var command = new ApplyDiscountCommand(request);
            var result = await _mediator.Send(command);

            if (!result.IsValid)
            {
                return BadRequest(ApiResponse<ApplyDiscountResponseDTO>.Fail(result.Message));
            }

            return Ok(ApiResponse<ApplyDiscountResponseDTO>.Ok(result));
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchDiscount([FromBody] SearchDiscountQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("get-by-entity-type")]
        public async Task<IActionResult> GetDiscountsByEntityType([FromBody] GetDiscountsByEntityTypeQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewDiscount([FromBody] AddNewDiscountCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDiscount(int id, [FromBody] UpdateDiscountDTO dto)
        {
            var command = new UpdateDiscountCommand(id, dto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("setup-default")]
        public async Task<IActionResult> SetupDefault([FromBody] SetupDiscountDefaultQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("setup-addnew")]
        public async Task<IActionResult> SetupAddnew([FromBody] SetupDiscountAddnewQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> SetupDisplay(int id)
        {
            var query = new SetupDiscountDisplayQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("setup-edit/{id}")]
        public async Task<IActionResult> SetupEdit(int id)
        {
            var query = new SetupDiscountEditQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiscount(int id)
        {
            var command = new DeleteDiscountCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
