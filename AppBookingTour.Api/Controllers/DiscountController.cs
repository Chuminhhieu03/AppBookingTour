using AppBookingTour.Application.Features.Discounts.AddNewDiscount;
using AppBookingTour.Application.Features.Discounts.SearchDiscounts;
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
    }
}
