using AppBookingTour.Application.Features.Accommodations.SetupAccommodationAddnew;
using AppBookingTour.Application.Features.Accommodations.SetupAccommodationDisplay;
using AppBookingTour.Application.Features.Accommodations.SetupAccommodationEdit;
using AppBookingTour.Application.Features.RoomTypes.AddNewRoomType;
using AppBookingTour.Application.Features.RoomTypes.SearchRoomTypes;
using AppBookingTour.Application.Features.RoomTypes.SetupRoomTypeAddnew;
using AppBookingTour.Application.Features.RoomTypes.UpdateRoomType;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppBookingTour.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomTypeController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RoomTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchRoomType([FromBody] SearchRoomTypeQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewRoomType([FromForm] AddNewRoomTypeDTO dto)
        {
            var command = new AddNewRoomTypeCommand(dto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoomType(int id, [FromBody] UpdateRoomTypeDTO dto)
        {
            var command = new UpdateRoomTypeCommand(id, dto);
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("setup-addnew")]
        public async Task<IActionResult> SetupAddnew([FromBody] SetupRoomTypeAddnewQuery query)
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
