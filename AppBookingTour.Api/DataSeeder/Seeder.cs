using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.Enums;
using AppBookingTour.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AppBookingTour.Api.DataSeeder
{
    public static class Seeder
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

            string[] roleNames = { "Admin", "Customer", "Business" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole<int>(roleName));
                }
            }
        }

        public static async Task SeedCitiesFromJsonAsync(IServiceProvider serviceProvider, string jsonFilePath)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Kiểm tra xem đã có Cities chưa
            if (await context.Cities.AnyAsync())
            {
                return; // Đã có dữ liệu, không seed nữa
            }

            if (!File.Exists(jsonFilePath))
            {
                throw new FileNotFoundException($"JSON file not found: {jsonFilePath}");
            }

            var jsonContent = await File.ReadAllTextAsync(jsonFilePath);
            var cityImportData = JsonSerializer.Deserialize<List<CityImportDto>>(jsonContent);

            if (cityImportData == null || !cityImportData.Any())
            {
                return;
            }

            var cities = new List<City>();

            foreach (var cityDto in cityImportData)
            {
                // Map Region string to enum
                if (!Enum.TryParse<Region>(cityDto.Region, ignoreCase: true, out var regionEnum))
                {
                    regionEnum = Region.North; // Default fallback
                }

                var city = new City
                {
                    Name = cityDto.Name,
                    Code = cityDto.Code,
                    Region = regionEnum,
                    IsPopular = cityDto.IsPopular,
                    Description = cityDto.Description,
                    ImageUrl = cityDto.ImageUrl,
                    IsActive = cityDto.IsActive,
                    Slug = cityDto.Slug,
                    CreatedAt = DateTime.UtcNow,
                };

                cities.Add(city);
            }

            await context.Cities.AddRangeAsync(cities);
            await context.SaveChangesAsync();
        }
    }
}