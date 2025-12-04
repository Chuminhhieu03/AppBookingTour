using AppBookingTour.Api.Contracts.Responses;
using AppBookingTour.Application.Features.RoomInventories.AddNewRoomInventory;
using AppBookingTour.Application.Features.RoomInventories.BulkAddRoomInventory;
using AppBookingTour.Application.Features.RoomInventories.BulkDeleteRoomInventory;
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

        /// <summary>
        /// Create room inventory for each day in range
        /// </summary>
        [HttpPost("bulk")]
        public async Task<ActionResult<ApiResponse<BulkAddRoomInventoryResponse>>> BulkAddRoomInventory(
            [FromBody] BulkAddRoomInventoryRequest request)
        {
            var result = await _mediator.Send(new BulkAddRoomInventoryCommand(request));

            if (!result.Success)
            {
                return BadRequest(ApiResponse<BulkAddRoomInventoryResponse>.Fail(result.Message));
            }

            return Ok(ApiResponse<BulkAddRoomInventoryResponse>.Ok(result));
        }

        /// <summary>
        /// Delete multiple room inventories by ids
        /// </summary>
        [HttpPost("bulk-delete")]
        public async Task<ActionResult<ApiResponse<BulkDeleteRoomInventoryResponse>>> BulkDeleteRoomInventory(
            [FromBody] BulkDeleteRoomInventoryRequest request)
        {
            var result = await _mediator.Send(new BulkDeleteRoomInventoryCommand(request));

            if (!result.Success)
            {
                return BadRequest(ApiResponse<BulkDeleteRoomInventoryResponse>.Fail(result.Message));
            }

            return Ok(ApiResponse<BulkDeleteRoomInventoryResponse>.Ok(result));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<AddNewRoomInventoryResponse>>> AddNewRoomInventory(
            [FromBody] AddNewRoomInventoryCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.Success)
            {
                return BadRequest(ApiResponse<AddNewRoomInventoryResponse>.Fail(result.Message));
            }

            return Ok(ApiResponse<AddNewRoomInventoryResponse>.Ok(result));
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
