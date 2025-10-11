using AppBookingTour.Application.Features.Auth.ResetPassword;
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

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] SearchDiscountQuery query)
        {
            var result = await _mediator.Send(query);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
