using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AppBookingTour.Api.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            //// Lấy ID của user từ JWT token
            //var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //if (!string.IsNullOrEmpty(userId))
            //{
            //    // Group theo Role - Dùng tên Role (string)
            //    if (Context.User!.IsInRole("Admin"))
            //    {
            //        await Groups.AddToGroupAsync(Context.ConnectionId, "Admin");
            //    }
            //    if (Context.User.IsInRole("Staff"))
            //    {
            //        await Groups.AddToGroupAsync(Context.ConnectionId, "Staff");
            //    }
            //    if (Context.User.IsInRole("Customer")) 
            //    {
            //        await Groups.AddToGroupAsync(Context.ConnectionId, "Customer");
            //        // Group cá nhân
            //        await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");
            //    }
            //    if (Context.User.IsInRole("Guide"))
            //    {
            //        await Groups.AddToGroupAsync(Context.ConnectionId, "Guide");
            //    }
            //}

            await Groups.AddToGroupAsync(Context.ConnectionId, "Group");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}