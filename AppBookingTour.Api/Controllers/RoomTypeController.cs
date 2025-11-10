using AppBookingTour.Application.Features.RoomTypes.AddNewRoomType;
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

        [HttpPost]
        public async Task<IActionResult> AddNewRoomType([FromForm] AddNewRoomTypeDTO dto)
        {
            var command = new AddNewRoomTypeCommand(dto);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoomType(int id, [FromForm] UpdateRoomTypeDTO dto)
        {
            var command = new UpdateRoomTypeCommand(id, dto);
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
