using AppBookingTour.Application.Features.Accommodations.AddNewAccommodation;
using AppBookingTour.Application.Features.Accommodations.SearchAccommodation;
using AppBookingTour.Application.Features.Accommodations.SetupAccommodationAddnew;
using AppBookingTour.Application.Features.Accommodations.SetupAccommodationDefault;
using AppBookingTour.Application.Features.Accommodations.SetupAccommodationDisplay;
using AppBookingTour.Application.Features.Accommodations.SetupAccommodationEdit;
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

        [HttpPost("setup-default")]
        public async Task<IActionResult> SetupDefault([FromBody] SetupAccommodationDefaultQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("setup-addnew")]
        public async Task<IActionResult> SetupAddnew([FromBody] SetupAccommodationAddnewQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> SetupDisplay(int id)
        {
            var query = new SetupAccommodationDisplayQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("setup-edit/{id}")]
        public async Task<IActionResult> SetupEdit(int id)
        {
            var query = new SetupAccommodationEditQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
