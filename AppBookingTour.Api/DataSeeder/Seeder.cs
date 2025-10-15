using Microsoft.AspNetCore.Identity;

namespace AppBookingTour.Api.DataSeeder
{
    public static class Seeder
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            // Lấy RoleManager từ service provider
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

            // Danh sách các role bạn muốn có trong hệ thống
            string[] roleNames = { "Admin", "Customer", "Staff" };

            foreach (var roleName in roleNames)
            {
                // Kiểm tra xem role đã tồn tại chưa
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // Nếu chưa, tạo role mới
                    await roleManager.CreateAsync(new IdentityRole<int>(roleName));
                }
            }
        }
    }
}