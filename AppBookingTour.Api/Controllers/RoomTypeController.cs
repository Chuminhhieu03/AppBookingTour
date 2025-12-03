using AppBookingTour.Application.Features.RoomTypes.AddNewRoomType;
using AppBookingTour.Application.Features.RoomTypes.DeleteRoomType;
using AppBookingTour.Application.Features.RoomTypes.GetRoomTypeById;
using AppBookingTour.Application.Features.RoomTypes.UpdateRoomType;
using AppBookingTour.Application.Features.RoomTypes.GetPreviewRoomTypeById;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetRoomTypeByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoomType(int id, [FromForm] UpdateRoomTypeDTO dto)
        {
            var command = new UpdateRoomTypeCommand(id, dto);
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoomType(int id)
        {
            var command = new DeleteRoomTypeCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("preview/{id}")]
        public async Task<IActionResult> GetPreviewById(int id)
        {
            var query = new GetPreviewRoomTypeByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
