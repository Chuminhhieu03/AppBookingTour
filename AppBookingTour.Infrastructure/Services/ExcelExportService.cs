using ClosedXML.Excel;
using AppBookingTour.Application.IServices;
using AppBookingTour.Application.Features.Statistics.ItemStatisticByRevenue;
using AppBookingTour.Application.Features.Statistics.ItemStatisticByBookingCount;

namespace AppBookingTour.Infrastructure.Services;

public class ExcelExportService : IExcelExportService
{
    // --- PHẦN 1: BÁO CÁO DOANH THU ---
    public byte[] ExportRevenueStatistics(List<ItemStatisticDTO> data, DateOnly startDate, DateOnly endDate, string itemTypeName)
    {
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("ThongKeDoanhThu");

            // 1. Tiêu đề lớn
            SetupReportHeader(worksheet, "THỐNG KÊ SẢN PHẨM THEO DOANH THU", 6);

            // 2. Thông tin chi tiết 
            SetupReportInfo(worksheet, itemTypeName, startDate, endDate, 6);

            // 3. Header bảng
            int row = 4;
            worksheet.Cell(row, 1).Value = "STT";
            worksheet.Cell(row, 2).Value = "Mã";
            worksheet.Cell(row, 3).Value = "Tên Sản Phẩm";
            worksheet.Cell(row, 4).Value = "Lượt Đặt";
            worksheet.Cell(row, 5).Value = "Doanh Thu";
            worksheet.Cell(row, 6).Value = "TB/Booking";

            StyleHeaderRow(worksheet, row, 6);

            // 4. Đổ dữ liệu
            row = 5;
            int stt = 1;
            foreach (var item in data)
            {
                SetCell(worksheet, row, 1, stt++, isCenter: true);
                SetCell(worksheet, row, 2, item.ItemCode, isCenter: true);
                SetCell(worksheet, row, 3, item.ItemName, isCenter: true);

                SetCell(worksheet, row, 4, item.totalCompletedBookings, isCenter: true);
                SetCell(worksheet, row, 5, item.TotalRevenue, format: "#,##0 \"đ\"");
                SetCell(worksheet, row, 6, item.averageRevenuePerBooking, format: "#,##0 \"đ\"");

                row++;
            }

            worksheet.Columns().AdjustToContents();
            return GetFileContent(workbook);
        }
    }

    // --- PHẦN 2: BÁO CÁO BOOKING COUNT ---
    public byte[] ExportBookingCountStatistics(List<ItemStatisticByBookingCountDTO> data, DateOnly startDate, DateOnly endDate, string itemTypeName)
    {
        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("ThongKeLuotDat");

            // 1. Tiêu đề lớn
            SetupReportHeader(worksheet, "THỐNG KÊ SẢN PHẨM THEO LƯỢT BOOKING", 7);

            // 2. Thông tin chi tiết
            SetupReportInfo(worksheet, itemTypeName, startDate, endDate, 7);

            // 3. Header bảng
            int row = 4;
            worksheet.Cell(row, 1).Value = "STT";
            worksheet.Cell(row, 2).Value = "Mã";
            worksheet.Cell(row, 3).Value = "Tên Sản Phẩm";
            worksheet.Cell(row, 4).Value = "Thành Công";
            worksheet.Cell(row, 5).Value = "Đã Hủy";
            worksheet.Cell(row, 6).Value = "Tổng Đặt";
            worksheet.Cell(row, 7).Value = "Tỉ Lệ Hủy";

            StyleHeaderRow(worksheet, row, 7);

            // 4. Đổ dữ liệu
            row = 5;
            int stt = 1;
            foreach (var item in data)
            {
                SetCell(worksheet, row, 1, stt++, isCenter: true);
                SetCell(worksheet, row, 2, item.ItemCode, isCenter: true);
                SetCell(worksheet, row, 3, item.ItemName, isCenter: true);

                SetCell(worksheet, row, 4, item.totalCompletedBookings, isCenter: true);
                SetCell(worksheet, row, 5, item.totalCanceledBookings, isCenter: true);

                var total = item.totalCompletedBookings + item.totalCanceledBookings;
                SetCell(worksheet, row, 6, total, isCenter: true);

                SetCell(worksheet, row, 7, item.cancellationRate, format: "0.00%");

                row++;
            }

            worksheet.Columns().AdjustToContents();
            return GetFileContent(workbook);
        }
    }

    // --- HELPER METHODS ---

    // [UPDATE] Hàm mới để setup thông tin trên cùng 1 dòng
    private void SetupReportInfo(IXLWorksheet worksheet, string itemTypeName, DateOnly startDate, DateOnly endDate, int colCount)
    {
        int midPoint = colCount / 2;

        // --- Block Trái: Loại sản phẩm 
        var rngType = worksheet.Range(2, 1, 2, midPoint);
        rngType.Merge().Value = $"Loại sản phẩm: {itemTypeName}";
        rngType.Style.Font.FontSize = 11;
        rngType.Style.Font.Italic = true;
        rngType.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

        // --- Block Phải: Thời gian
        var rngDate = worksheet.Range(2, midPoint + 1, 2, colCount);
        rngDate.Merge().Value = $"Giai đoạn: {startDate:dd/MM/yyyy} - {endDate:dd/MM/yyyy}";
        rngDate.Style.Font.FontSize = 11;
        rngDate.Style.Font.Italic = true;
        rngDate.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

        worksheet.Row(2).Height = 20;
    }

    private void SetCell(IXLWorksheet ws, int row, int col, object value, bool isCenter = false, string? format = null)
    {
        var cell = ws.Cell(row, col);
        cell.Value = XLCellValue.FromObject(value);

        if (isCenter)
        {
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        }
        else if (!string.IsNullOrEmpty(format))
        {
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
        }

        if (!string.IsNullOrEmpty(format))
        {
            cell.Style.NumberFormat.Format = format;
        }
    }

    private void SetupReportHeader(IXLWorksheet worksheet, string title, int colCount)
    {
        var rng = worksheet.Range(1, 1, 1, colCount);
        rng.Merge().Value = title.ToUpper();
        rng.Style.Font.Bold = true;
        rng.Style.Font.FontSize = 14;
        rng.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        rng.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
        worksheet.Row(1).Height = 25;
    }

    private void StyleHeaderRow(IXLWorksheet worksheet, int row, int colCount)
    {
        var rng = worksheet.Range(row, 1, row, colCount);
        rng.Style.Font.Bold = true;
        rng.Style.Fill.BackgroundColor = XLColor.CornflowerBlue;
        rng.Style.Font.FontColor = XLColor.White;
        rng.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        rng.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
    }

    private byte[] GetFileContent(XLWorkbook workbook)
    {
        using (var stream = new MemoryStream())
        {
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}