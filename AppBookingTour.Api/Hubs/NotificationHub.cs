using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace AppBookingTour.Api.Hubs
{
    // Hub class kế thừa từ Hub
    public class NotificationHub : Hub
    {
        // Ghi log khi có client kết nối
        public override async Task OnConnectedAsync()
        {
            // (Tùy chọn) Bạn có thể lấy user ID từ context nếu user đã đăng nhập
            // var userId = Context.UserIdentifier; 

            // Gửi thông báo chào mừng chỉ đến client vừa kết nối
            await Clients.Caller.SendAsync("ReceiveWelcomeMessage", "Chào mừng bạn đã kết nối tới hệ thống thông báo!");

            // Ghi log ra console (hoặc logger của bạn)
            Console.WriteLine($"Client connected: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        // Ghi log khi client ngắt kết nối
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"Client disconnected: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(exception);
        }

        // (Demo) Một phương thức mà client có thể gọi lên server
        // Ví dụ: client gửi 1 tin nhắn, server broadcast cho tất cả
        public async Task SendMessageToAll(string user, string message)
        {
            // "ReceiveMessage" là tên sự kiện mà các client sẽ lắng nghe
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}