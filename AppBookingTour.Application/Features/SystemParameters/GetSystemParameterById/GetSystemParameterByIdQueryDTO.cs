
namespace AppBookingTour.Application.Features.SystemParameters.GetSystemParameterById;

public class GetSystemParameterByIdResponse
{
    public bool IsSuccess { get; init; }
    public string? ErrorMessage { get; init; }
    public SystemParameterDTO? SystemParameter { get; init; }

    public static GetSystemParameterByIdResponse Success(SystemParameterDTO systemParameter) =>
        new() { IsSuccess = true, SystemParameter = systemParameter };

    public static GetSystemParameterByIdResponse Failed(string errorMessage) =>
        new() { IsSuccess = false, ErrorMessage = errorMessage };
}

public class SystemParameterDTO
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string Name { get; set; } = null!;
    public int FeatureCode { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
