using AppBookingTour.Application.Features.SystemParameters.GetSystemParameterById;

namespace AppBookingTour.Application.Features.SystemParameters.CreateSystemParameter
{
    public class CreateSystemParameterResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public SystemParameterDTO? SystemParameter { get; set; }

        public static CreateSystemParameterResponse Success(SystemParameterDTO systemParameter) =>
            new() { IsSuccess = true, SystemParameter = systemParameter};

        public static CreateSystemParameterResponse Fail(string message) =>
            new() { IsSuccess = false, Message = message };
    }

    public class SystemParameterRequestDTO
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? FeatureCode { get; set; }
        public string? Description { get; set; }
    }
}