using AppBookingTour.Application.Features.Cities.GetCityById;

namespace AppBookingTour.Application.Features.Cities.GetListCity;

public class GetListCityResponse
{
    public bool IsSuccess { get; init; }
    public string? Message { get; init; }
    public List<CityDTO> Cities { get; init; } = new();

    public static GetListCityResponse Success(List<CityDTO> cities) =>
        new() { IsSuccess = true, Cities = cities };

    public static GetListCityResponse Failed(string message) =>
        new() { IsSuccess = false, Message = message };
}