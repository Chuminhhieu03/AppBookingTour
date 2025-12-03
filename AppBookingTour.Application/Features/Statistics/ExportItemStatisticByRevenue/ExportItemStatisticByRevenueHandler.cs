using AppBookingTour.Application.IRepositories;
using AppBookingTour.Application.IServices;
using MediatR;

namespace AppBookingTour.Application.Features.Statistics.ExportItemStatisticByRevenue;

public class ExportItemStatisticByRevenueHandler : IRequestHandler<ExportItemStatisticByRevenueQuery, ExportFileDTO>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExcelExportService _excelService;

    public ExportItemStatisticByRevenueHandler(IUnitOfWork unitOfWork, IExcelExportService excelService)
    {
        _unitOfWork = unitOfWork;
        _excelService = excelService;
    }

    public async Task<ExportFileDTO> Handle(ExportItemStatisticByRevenueQuery request, CancellationToken cancellationToken)
    {
        var isDesc = request.IsDesc ?? true;

        var (items, _, _) = await _unitOfWork.Statistics.GetItemRevenueStatisticsAsync(
            request.StartDate,
            request.EndDate,
            request.ItemType,
            pageIndex: null,
            pageSize: null,
            isDesc: isDesc,
            cancellationToken: cancellationToken);

        var fileContent = _excelService.ExportRevenueStatistics(
            items.ToList(),
            request.StartDate,
            request.EndDate,
            request.ItemType.ToString()
        );

        return new ExportFileDTO
        {
            FileName = $"BaoCao_DoanhThu_{request.ItemType}_{request.StartDate:yyyyMMdd}_{request.EndDate:yyyyMMdd}.xlsx",
            Data = fileContent,
            ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        };
    }
}