using AppBookingTour.Api.Contracts.Responses;
using AppBookingTour.Application.Features.Accommodations.AddNewAccommodation;
using AppBookingTour.Application.Features.Accommodations.DeleteAccommodation;
using AppBookingTour.Application.Features.Accommodations.GetAccommodationById;
using AppBookingTour.Application.Features.Accommodations.GetAccommodationForCustomerById;
using AppBookingTour.Application.Features.Accommodations.SearchAccommodation;
using AppBookingTour.Application.Features.Accommodations.SearchAccommodationsForCustomer;
using AppBookingTour.Application.Features.Accommodations.UpdateAccommodation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppBookingTour.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccommodationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccommodationController(IMediator mediator, ILogger<AuthController> logger)
        {
            _mediator = mediator;
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchAccommodation([FromBody] SearchAccomodationQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("search-for-customer")]
        public async Task<IActionResult> SearchAccommodationForCustomer([FromBody] SearchAccommodationsForCustomerQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(ApiResponse<object>.Ok(result));
        }

        [HttpPost]
        public async Task<IActionResult> AddNewAccommodation([FromForm] AddNewAccommodationDTO dto)
        {
            var command = new AddNewAccommodationCommand(dto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccommodation(int id, [FromForm] UpdateAccommodationDTO dto)
        {
            var command = new UpdateAccommodationCommand(id, dto);
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetAccommodationByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("customer/{id}")]
        public async Task<IActionResult> GetAccommodationForCustomerById(int id)
        {
            var query = new GetAccommodationForCustomerByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAccommodation(int id)
		{
			var command = new DeleteAccommodationCommand(id);
			var result = await _mediator.Send(command);
			return Ok(result);
		}
    }
}
