
using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Application.Features.Cities.GetCityById;

public class GetCityByIdResponse
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public CityDTO? City { get; init; }

    public static GetCityByIdResponse Success(CityDTO city) =>
        new() { IsSuccess = true, City = city };

    public static GetCityByIdResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, ErrorMessage = errorMessage };
}

public class CityDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Code { get; set; }
    public Region? Region { get; set; }
    public string? RegionName { get; set; }
    public bool IsPopular { get; set; }
    public string Slug { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}