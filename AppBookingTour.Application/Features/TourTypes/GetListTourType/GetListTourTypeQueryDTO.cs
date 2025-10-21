using AppBookingTour.Application.Features.TourTypes.GetTourTypeById;

namespace AppBookingTour.Application.Features.TourTypes.GetTourTypesList;

public class GetTourTypesListResponse
{
    public bool IsSuccess { get; init; }
    public string? Message { get; init; }

    public List<TourTypeDTO> TourTypes { get; init; } = new();


    public static GetTourTypesListResponse Success(List<TourTypeDTO> tourTypes) =>
        new() { IsSuccess = true, TourTypes = tourTypes, Message = "Retrieved tour types successfully." };

    public static GetTourTypesListResponse Failed(string message) =>
        new() { IsSuccess = false, Message = message };
}