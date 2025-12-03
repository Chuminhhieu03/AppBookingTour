
namespace AppBookingTour.Application.Features.Statistics.ExportItemStatisticByRevenue;

public class ExportFileDTO
{
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    public byte[] Data { get; set; } = Array.Empty<byte>();
}
