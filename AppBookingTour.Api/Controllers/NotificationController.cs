using AppBookingTour.Api.Contracts.Responses;
using AppBookingTour.Api.Hubs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace AppBookingTour.Api.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        // Inject IHubContext
        public NotificationController(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        /// <summary>
        /// (Demo) Endpoint này để gửi thông báo "Hello World"
        /// tới TẤT CẢ các client đang kết nối.
        /// </summary>
        [HttpPost("broadcast")]
        public async Task<IActionResult> BroadcastMessage([FromQuery] string message)
        {
            try
            {
                // "ReceiveNotification" là tên SỰ KIỆN mà client sẽ lắng nghe
                // Bạn có thể gửi bất kỳ object nào
                await _hubContext.Clients.All.SendAsync("ReceiveNotification", new
                {
                    type = "Global",
                    content = message,
                    timestamp = DateTime.UtcNow
                });

                return Ok(ApiResponse<object>.Ok(null, "Tin nhắn đã được gửi đi"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }
    }
}