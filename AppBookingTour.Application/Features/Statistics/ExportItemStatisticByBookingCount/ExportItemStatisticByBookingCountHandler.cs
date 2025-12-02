using MediatR;
using AppBookingTour.Application.IServices;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.Features.Statistics.ExportItemStatisticByRevenue;

namespace AppBookingTour.Application.Features.Statistics.ExportItemStatisticByBookingCount;

public class ExportItemStatisticByBookingCountHandler : IRequestHandler<ExportItemStatisticByBookingCountQuery, ExportFileDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExcelExportService _excelService;

    public ExportItemStatisticByBookingCountHandler(IUnitOfWork unitOfWork, IExcelExportService excelService)
    {
        _unitOfWork = unitOfWork;
        _excelService = excelService;
    }

    public async Task<ExportFileDTO> Handle(ExportItemStatisticByBookingCountQuery request, CancellationToken cancellationToken)
    {
        var isDesc = request.IsDesc ?? true;

        var (items, _, _) = await _unitOfWork.Statistics.GetItemBookingCountStatisticsAsync(
            request.StartDate,
            request.EndDate,
            request.ItemType,
            pageIndex: null,
            pageSize: null,
            isDesc: isDesc,
            cancellationToken: cancellationToken);

        var fileContent = _excelService.ExportBookingCountStatistics(
            items.ToList(),
            request.StartDate,
            request.EndDate,
            request.ItemType.ToString()
        );

        return new ExportFileDTO
        {
            FileName = $"BaoCao_LuotBooking_{request.ItemType}_{request.StartDate:yyyyMMdd}_{request.EndDate:yyyyMMdd}.xlsx",
            Data = fileContent,
            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        };
    }
}