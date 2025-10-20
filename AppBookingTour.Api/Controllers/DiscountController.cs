using AppBookingTour.Api.Contracts.Responses;
using AppBookingTour.Application.Features.Discounts.AddNewDiscount;
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

        public DiscountController(IMediator mediator, ILogger<AuthController> logger)
        {
            _mediator = mediator;
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchDiscount([FromBody] SearchDiscountQuery query)
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
    }
}
