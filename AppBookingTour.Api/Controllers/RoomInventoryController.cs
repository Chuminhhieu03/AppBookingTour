using AppBookingTour.Application.Features.RoomInventories.AddNewRoomInventory;
using AppBookingTour.Application.Features.RoomInventories.DeleteRoomInventory;
using AppBookingTour.Application.Features.RoomInventories.SearchRoomInventories;
using AppBookingTour.Application.Features.RoomInventories.UpdateRoomInventory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppBookingTour.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomInventoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RoomInventoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchRoomInventory([FromBody] SearchRoomInventoryQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewRoomInventory([FromBody] AddNewRoomInventoryCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoomInventory(int id, [FromBody] UpdateRoomInventoryDTO dto)
        {
            var command = new UpdateRoomInventoryCommand(id, dto);
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoomInventory(int id)
        {
            var command = new DeleteRoomInventoryCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
