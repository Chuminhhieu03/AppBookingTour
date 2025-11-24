using AppBookingTour.Application.Features.Statistics.ItemBookingCountDetail;
using AppBookingTour.Application.Features.Statistics.ItemRevenueDetail;
using AppBookingTour.Application.Features.Statistics.ItemStatisticByBookingCount;
using AppBookingTour.Application.Features.Statistics.ItemStatisticByRevenue;
using AppBookingTour.Application.Features.Statistics.OverviewStatistic;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Enums;
using AppBookingTour.Infrastructure.Database;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AppBookingTour.Infrastructure.Data.Repositories
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly ApplicationDbContext _context;

        // Status booking:
        // Thành công là Paid = 3
        // Bị hủy Cancelled = 5 => bằng bất kì lí do gì (người dùng hoặc là hết thời gian)
        private readonly int[] _successStatuses = new int[] { 3 }; // Paid
        private readonly int[] _canceledStatuses = new int[] { 5 }; // Cancelled
        private readonly int[] _allRelevantStatuses = new int[] { 3, 5 };

        public StatisticsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ItemStatisticDTO>> GetItemRevenueStatisticsAsync(
            DateOnly startDate,
            DateOnly endDate,
            ItemType itemType,
            CancellationToken cancellationToken = default)
        {
            string sql;

            var parameters = new
            {
                startDate = startDate.ToDateTime(TimeOnly.MinValue),
                endDate = endDate.ToDateTime(TimeOnly.MaxValue),
                bookingType = (int)itemType,
                sttSuccess = _successStatuses,
                sttAll = _allRelevantStatuses
            };

            switch (itemType)
            {
                case ItemType.Tour:
                    sql = @"
                         SELECT
                            T.Id AS ItemId,
                            T.Code AS ItemCode,
                            T.Name AS ItemName,
                            T.Rating AS rating,
                            COUNT(CASE WHEN B.Status IN @sttSuccess THEN B.Id ELSE NULL END) AS totalCompletedBookings,
                            ISNULL(SUM(CASE WHEN B.Status IN @sttSuccess THEN B.FinalAmount ELSE 0 END), 0) AS TotalRevenue,
                            COUNT(DISTINCT B.TravelDate) AS ItemOptionCount 
                        FROM 
                            Bookings AS B
                        JOIN 
                            Tours AS T ON B.ItemId = T.Id 
                        WHERE
                            B.BookingType = @bookingType
                            AND B.BookingDate BETWEEN @startDate AND @endDate
                            AND B.Status IN @sttAll
                        GROUP BY 
                            T.Id, T.Code, T.Name, T.Rating
                    ";
                    break;

                case ItemType.Accommodation:
                    sql = @"
                        SELECT
                            A.Id AS ItemId,
                            A.Code AS ItemCode,
                            A.Name AS ItemName,
                            A.Rating AS rating,
                            COUNT(CASE WHEN B.Status IN @sttSuccess THEN B.Id ELSE NULL END) AS totalCompletedBookings,
                            ISNULL(SUM(CASE WHEN B.Status IN @sttSuccess THEN B.FinalAmount ELSE 0 END), 0) AS TotalRevenue,
                            COUNT(DISTINCT RT.Id) AS ItemOptionCount 
                        FROM
                            Bookings AS B
                        JOIN
                            RoomTypes AS RT ON B.ItemId = RT.Id 
                        JOIN
                            Accommodations AS A ON RT.AccommodationId = A.Id
                        WHERE
                            B.BookingType = @bookingType
                            AND B.BookingDate BETWEEN @startDate AND @endDate 
                            AND B.Status IN @sttAll
                        GROUP BY
                            A.Id, A.Code, A.Name, A.Rating
                    ";
                    break;

                case ItemType.Combo:
                    sql = @"
                        SELECT
                            C.Id AS ItemId,
                            C.Code AS ItemCode,
                            C.Name AS ItemName,
                            C.Rating AS rating,
                            COUNT(CASE WHEN B.Status IN @sttSuccess THEN B.Id ELSE NULL END) AS totalCompletedBookings,
                            ISNULL(SUM(CASE WHEN B.Status IN @sttSuccess THEN B.FinalAmount ELSE 0 END), 0) AS TotalRevenue,
                            COUNT(DISTINCT B.TravelDate) AS ItemOptionCount
                        FROM 
                            Bookings AS B
                        JOIN 
                            Combos AS C ON B.ItemId = C.Id
                        WHERE
                            B.BookingType = @bookingType
                            AND B.BookingDate BETWEEN @startDate AND @endDate
                            AND B.Status IN @sttAll
                        GROUP BY 
                            C.Id, C.Code, C.Name, C.Rating
                    ";
                    break;

                default:
                    return Enumerable.Empty<ItemStatisticDTO>();
            }

            var rawResults = await _context.Database.GetDbConnection()
                .QueryAsync<RawStatisticResult>(sql, parameters);

            var finalResults = rawResults.Select(r => new ItemStatisticDTO
            {
                ItemId = r.ItemId,
                ItemCode = r.ItemCode,
                ItemName = r.ItemName,
                rating = r.rating,
                totalCompletedBookings = r.totalCompletedBookings,
                TotalRevenue = r.TotalRevenue!.Value,
                averageRevenuePerBooking = (r.totalCompletedBookings == 0) ? 0 : r.TotalRevenue!.Value / r.totalCompletedBookings,
                ItemOptionCount = r.ItemOptionCount,
            })
            .OrderByDescending(r => r.TotalRevenue)
            .ToList();

            return finalResults;
        }

        public async Task<IEnumerable<ItemRevenueDetailDTO>> GetItemRevenueDetailAsync(
            DateOnly startDate,
            DateOnly endDate,
            ItemType itemType,
            int itemId,
            CancellationToken cancellationToken = default)
        {
            string sql;

            var parameters = new
            {
                startDate = startDate.ToDateTime(TimeOnly.MinValue),
                endDate = endDate.ToDateTime(TimeOnly.MinValue),
                bookingType = (int)itemType,
                itemId,
                sttSuccess = _successStatuses,
                sttAll = _allRelevantStatuses
            };

            switch (itemType)
            {
                case ItemType.Tour:
                case ItemType.Combo:
                    sql = @"
                        WITH RevenueData AS (
                            SELECT
                                B.TravelDate AS DepartureDate,
                                FORMAT(B.TravelDate, 'yyyy-MM-dd') AS ItemDetailName,
                                COUNT(CASE WHEN B.Status IN @sttSuccess THEN B.Id ELSE NULL END) AS totalCompletedBookings,
                                ISNULL(SUM(CASE WHEN B.Status IN @sttSuccess THEN B.FinalAmount ELSE 0 END), 0) AS TotalRevenue
                            FROM
                                Bookings AS B
                            WHERE
                                B.ItemId = @itemId
                                AND B.BookingType = @bookingType
                                AND B.Status IN @sttAll
                                AND CAST(B.BookingDate AS DATE) BETWEEN @startDate AND @endDate
                            GROUP BY
                                B.TravelDate
                        )
                        SELECT
                            RD.ItemDetailName,
                            RD.totalCompletedBookings,
                            RD.TotalRevenue,
                            CASE WHEN RD.totalCompletedBookings > 0 
                                 THEN (RD.TotalRevenue / RD.totalCompletedBookings) 
                                 ELSE 0 
                            END AS averageRevenuePerBooking
                        FROM
                            RevenueData AS RD
                        WHERE
                            RD.TotalRevenue > 0 OR RD.totalCompletedBookings > 0
                        ORDER BY
                            RD.DepartureDate;
                    ";
                    break;

                case ItemType.Accommodation:
                    sql = @"
                        WITH RevenueData AS (
                            SELECT
                                RT.Id,
                                RT.Name AS ItemDetailName,
                                COUNT(CASE WHEN B.Status IN @sttSuccess THEN B.Id ELSE NULL END) AS totalCompletedBookings,
                                ISNULL(SUM(CASE WHEN B.Status IN @sttSuccess THEN B.FinalAmount ELSE 0 END), 0) AS TotalRevenue
                            FROM
                                RoomTypes AS RT
                            LEFT JOIN
                                Bookings AS B ON RT.Id = B.ItemId
                                    AND B.BookingType = @bookingType
                                    AND B.Status IN @sttAll
                                    AND CAST(B.BookingDate AS DATE) BETWEEN @startDate AND @endDate
                            WHERE
                                RT.AccommodationId = @itemId
                            GROUP BY
                                RT.Id, RT.Name
                        )
                        SELECT
                            RD.ItemDetailName,
                            RD.totalCompletedBookings,
                            RD.TotalRevenue,
                            (RD.TotalRevenue / RD.totalCompletedBookings) AS averageRevenuePerBooking
                        FROM
                            RevenueData AS RD
                        WHERE
                            RD.totalCompletedBookings > 0
                        ORDER BY
                            RD.ItemDetailName;
                    ";
                    break;
        
                default:
                    return Enumerable.Empty<ItemRevenueDetailDTO>();
            }

            var results = await _context.Database.GetDbConnection()
                .QueryAsync<ItemRevenueDetailDTO>(sql, parameters);

            return results;
        }

        public async Task<IEnumerable<ItemStatisticByBookingCountDTO>> GetItemBookingCountStatisticsAsync(
            DateOnly startDate,
            DateOnly endDate,
            ItemType itemType,
            CancellationToken cancellationToken = default)
        {
            string sql;

            var parameters = new
            {
                startDate = startDate.ToDateTime(TimeOnly.MinValue),
                endDate = endDate.ToDateTime(TimeOnly.MinValue),
                bookingType = (int)itemType,
                sttSuccess = _successStatuses,
                sttCanceled = _canceledStatuses,
                sttAll = _allRelevantStatuses
            };

            switch (itemType)
            {
                case ItemType.Tour:
                    sql = @"
                        SELECT
                            T.Id AS ItemId, 
                            T.Code AS ItemCode, 
                            T.Name AS ItemName, 
                            T.Rating AS rating,
                            COUNT(CASE WHEN B.Status IN @sttSuccess THEN B.Id ELSE NULL END) AS totalCompletedBookings,
                            COUNT(CASE WHEN B.Status IN @sttCanceled THEN B.Id ELSE NULL END) AS totalCanceledBookings,
                            COUNT(CASE WHEN B.Status IN @sttAll THEN B.Id ELSE NULL END) AS TotalBookingsCount,
                            COUNT(DISTINCT B.TravelDate) AS ItemOptionCount
                         FROM 
                            Bookings AS B
                         JOIN 
                            Tours AS T ON B.ItemId = T.Id
                         WHERE 
                            B.BookingType = @bookingType
                            AND CAST(B.BookingDate AS DATE) BETWEEN @startDate AND @endDate
                            AND B.Status IN @sttAll
                         GROUP BY 
                            T.Id, T.Code, T.Name, T.Rating
                    ";
                    break;

                case ItemType.Accommodation:
                    sql = @"
                        SELECT
                            A.Id AS ItemId, 
                            A.Code AS ItemCode, 
                            A.Name AS ItemName, 
                            A.Rating AS rating,
                            COUNT(CASE WHEN B.Status IN @sttSuccess THEN B.Id ELSE NULL END) AS totalCompletedBookings,
                            COUNT(CASE WHEN B.Status IN @sttCanceled THEN B.Id ELSE NULL END) AS totalCanceledBookings,
                            COUNT(CASE WHEN B.Status IN @sttAll THEN B.Id ELSE NULL END) AS TotalBookingsCount,
                            COUNT(DISTINCT RT.Id) AS ItemOptionCount 
                        FROM 
                            Bookings AS B
                        JOIN 
                            RoomTypes AS RT ON B.ItemId = RT.Id 
                        JOIN 
                            Accommodations AS A ON RT.AccommodationId = A.Id
                        WHERE 
                            B.BookingType = @bookingType
                            AND CAST(B.BookingDate AS DATE) BETWEEN @startDate AND @endDate 
                            AND B.Status IN @sttAll
                        GROUP 
                            BY A.Id, A.Code, A.Name, A.Rating
                    ";
                    break;

                case ItemType.Combo:
                    sql = @"
                        SELECT
                            C.Id AS ItemId, 
                            C.Code AS ItemCode, 
                            C.Name AS ItemName, 
                            C.Rating AS rating,
                            COUNT(CASE WHEN B.Status IN @sttSuccess THEN B.Id ELSE NULL END) AS totalCompletedBookings,
                            COUNT(CASE WHEN B.Status IN @sttCanceled THEN B.Id ELSE NULL END) AS totalCanceledBookings,
                            COUNT(CASE WHEN B.Status IN @sttAll THEN B.Id ELSE NULL END) AS TotalBookingsCount,
                            COUNT(DISTINCT B.TravelDate) AS ItemOptionCount
                         FROM 
                            Bookings AS B
                         JOIN 
                            Combos AS C ON B.ItemId = C.Id
                         WHERE 
                            B.BookingType = @bookingType
                            AND CAST(B.BookingDate AS DATE) BETWEEN @startDate AND @endDate
                            AND B.Status IN @sttAll
                         GROUP BY 
                            C.Id, C.Code, C.Name, C.Rating
                    ";
                    break;

                default:
                    return Enumerable.Empty<ItemStatisticByBookingCountDTO>();
            }

            var rawResults = await _context.Database.GetDbConnection()
                .QueryAsync<RawStatisticResult>(sql, parameters);

            var finalResults = rawResults.Select(r => new ItemStatisticByBookingCountDTO
            {
                ItemId = r.ItemId,
                ItemCode = r.ItemCode,
                ItemName = r.ItemName,
                totalCompletedBookings = r.totalCompletedBookings,
                totalCanceledBookings = r.totalCanceledBookings.HasValue ? r.totalCanceledBookings.Value : 0,
                ItemOptionCount = r.ItemOptionCount,
                rating = r.rating,
                cancellationRate = (r.totalCanceledBookings.HasValue && r.TotalBookingsCount.HasValue && r.TotalBookingsCount.Value > 0)
                                    ? Math.Round((decimal)r.totalCanceledBookings.Value / r.TotalBookingsCount.Value, 4) : 0,
            })
            .OrderByDescending(r => r.totalCompletedBookings)
            .ToList();

            return finalResults;
        }

        public async Task<IEnumerable<ItemBookingCountDetailDTO>> GetItemBookingCountDetailAsync(
            DateOnly startDate,
            DateOnly endDate,
            ItemType itemType,
            int itemId,
            CancellationToken cancellationToken = default)
        {
            string sql;

            var parameters = new
            {
                startDate = startDate.ToDateTime(TimeOnly.MinValue),
                endDate = endDate.ToDateTime(TimeOnly.MinValue),
                bookingType = (int)itemType,
                itemId,
                sttSuccess = _successStatuses,
                sttCanceled = _canceledStatuses,
                sttAll = _allRelevantStatuses
            };

            switch (itemType)
            {
                case ItemType.Tour:
                case ItemType.Combo:
                    sql = @"
                        WITH BookingCounts AS (
                            SELECT
                                B.TravelDate AS DepartureDate,
                                FORMAT(B.TravelDate, 'yyyy-MM-dd') AS ItemDetailName,
                                COUNT(CASE WHEN B.Status IN @sttSuccess THEN B.Id ELSE NULL END) AS totalCompletedBookings,
                                COUNT(CASE WHEN B.Status IN @sttCanceled THEN B.Id ELSE NULL END) AS totalCanceledBookings
                            FROM
                                Bookings AS B
                            WHERE
                                B.ItemId = @itemId
                                AND B.BookingType = @bookingType
                                AND B.Status IN @sttAll
                                AND CAST(B.BookingDate AS DATE) BETWEEN @startDate AND @endDate
                            GROUP BY
                                B.TravelDate
                        )
                        SELECT
                            BC.ItemDetailName,
                            BC.totalCompletedBookings,
                            BC.totalCanceledBookings,
                            (BC.totalCanceledBookings * 100.0) / (BC.totalCompletedBookings + BC.totalCanceledBookings) AS cancellationRate
                        FROM
                            BookingCounts AS BC
                        WHERE
                            (BC.totalCompletedBookings + BC.totalCanceledBookings) > 0
                        ORDER BY
                            BC.DepartureDate;
                    ";
                    break;

                case ItemType.Accommodation:
                    sql = @"
                        WITH BookingCounts AS (
                            SELECT
                                RT.Id,
                                RT.Name AS ItemDetailName,
                                COUNT(CASE WHEN B.Status IN @sttSuccess THEN B.Id ELSE NULL END) AS totalCompletedBookings,
                                COUNT(CASE WHEN B.Status IN @sttCanceled THEN B.Id ELSE NULL END) AS totalCanceledBookings
                            FROM
                                RoomTypes AS RT
                            LEFT JOIN
                                Bookings AS B ON RT.Id = B.ItemId
                                    AND B.BookingType = @bookingType
                                    AND B.Status IN @sttAll
                                    AND CAST(B.BookingDate AS DATE) BETWEEN @startDate AND @endDate
                            WHERE
                                RT.AccommodationId = @itemId
                            GROUP BY
                                RT.Id, RT.Name
                        )
                        SELECT
                            BC.ItemDetailName,
                            BC.totalCompletedBookings,
                            BC.totalCanceledBookings,
                            (BC.totalCanceledBookings * 100.0) / (BC.totalCompletedBookings + BC.totalCanceledBookings) AS cancellationRate
                        FROM
                            BookingCounts AS BC
                        WHERE
                            (BC.totalCompletedBookings + BC.totalCanceledBookings) > 0
                        ORDER BY
                            BC.ItemDetailName;
                    ";
                    break;

                default:
                    return Enumerable.Empty<ItemBookingCountDetailDTO>();
            }

            var results = await _context.Database.GetDbConnection()
                .QueryAsync<ItemBookingCountDetailDTO>(sql, parameters);

            return results;
        }

        public async Task<OverviewStatisticDTO> GetOverviewStatisticsAsync(
             DateTime currentPeriodStart,
             DateTime currentPeriodEnd,
             DateTime previousPeriodStart,
             DateTime previousPeriodEnd,
             DateTime yearStart,
             CancellationToken cancellationToken = default)
        {
            var parameters = new
            {
                CurrentPeriodStart = currentPeriodStart,
                CurrentPeriodEnd = currentPeriodEnd,
                PreviousPeriodStart = previousPeriodStart,
                PreviousPeriodEnd = previousPeriodEnd,
                YearStart = yearStart,
                sttSuccess = _successStatuses,
                sttCanceled = _canceledStatuses,
                sttAll = _allRelevantStatuses
            };

            var sql = @"
                WITH YearlyReportData AS (
                    SELECT
                        MONTH(BookingDate) AS ReportMonth,
                        YEAR(BookingDate) AS ReportYear,
                        BookingType,
                        ISNULL(SUM(CASE WHEN Status IN @sttSuccess THEN FinalAmount ELSE 0 END), 0) AS TotalRevenue,
                        COUNT(CASE WHEN Status IN @sttSuccess THEN 1 ELSE NULL END) AS CompletedBookings
                    FROM 
                        Bookings
                    WHERE
                        BookingDate BETWEEN @YearStart AND @CurrentPeriodEnd
                        AND Status IN @sttSuccess 
                    GROUP BY
                        YEAR(BookingDate), MONTH(BookingDate), BookingType
                )
                SELECT * FROM YearlyReportData;

                WITH CurrentMonthData AS (
                    SELECT
                        BookingType,
                        ISNULL(SUM(CASE WHEN Status IN @sttSuccess THEN FinalAmount ELSE 0 END), 0) AS TotalRevenue,
                        COUNT(Id) AS TotalBookings, 
                        COUNT(CASE WHEN Status IN @sttSuccess THEN 1 ELSE NULL END) AS TotalCompletedBookings,
                        COUNT(CASE WHEN Status IN @sttCanceled THEN 1 ELSE NULL END) AS TotalCanceledBookings
                    FROM 
                        Bookings
                    WHERE
                        BookingDate BETWEEN @CurrentPeriodStart AND @CurrentPeriodEnd
                        AND Status IN @sttAll 
                    GROUP BY
                        BookingType
                )
                SELECT * FROM CurrentMonthData;

                WITH PreviousMonthData AS (
                    SELECT
                        ISNULL(SUM(FinalAmount), 0) AS TotalRevenue
                    FROM 
                        Bookings
                    WHERE
                        BookingDate BETWEEN @PreviousPeriodStart AND @PreviousPeriodEnd
                        AND Status IN @sttSuccess 
                )
                SELECT TotalRevenue AS PreviousMonthRevenue FROM PreviousMonthData;
            ";

            var connection = _context.Database.GetDbConnection();

            using (var grid = await connection.QueryMultipleAsync(sql, parameters))
            {
                var yearlyData = (await grid.ReadAsync<YearlyAggregateDto>()).ToList();
                var currentMonthData = (await grid.ReadAsync<CurrentMonthAggregateDto>()).ToList();
                var previousMonthRevenue = await grid.ReadSingleAsync<decimal>();

                var overviewDto = new OverviewStatisticDTO
                {
                    Month = currentPeriodEnd.Month,
                    Year = currentPeriodEnd.Year,
                    PreviousMonthRevenue = previousMonthRevenue,
                    TotalRevenue = currentMonthData.Sum(x => x.TotalRevenue),
                    TotalBookings = currentMonthData.Sum(x => x.TotalBookings),
                    CompletedBookings = currentMonthData.Sum(x => x.TotalCompletedBookings),
                    CanceledBookings = currentMonthData.Sum(x => x.TotalCanceledBookings),
                    SummaryByType = new SummaryByTypeDTO
                    {
                        Tour = MapToTypeStatistic(currentMonthData, BookingType.Tour),
                        Accommodation = MapToTypeStatistic(currentMonthData, BookingType.Accommodation),
                        Combo = MapToTypeStatistic(currentMonthData, BookingType.Combo)
                    },
                    MonthlyReport = MapToMonthlyReport(yearlyData, currentPeriodEnd.Year)
                };

                if (overviewDto.PreviousMonthRevenue > 0)
                {
                    overviewDto.GrowthRate = Math.Round(
                        (((double)overviewDto.TotalRevenue - (double)overviewDto.PreviousMonthRevenue) / (double)overviewDto.PreviousMonthRevenue) * 100,
                        2
                    );
                }
                else if (overviewDto.TotalRevenue > 0)
                {
                    overviewDto.GrowthRate = 100.0;
                }
                else
                {
                    overviewDto.GrowthRate = 0;
                }

                return overviewDto;
            }
        }

        private TypeStatisticDTO MapToTypeStatistic(List<CurrentMonthAggregateDto> data, BookingType type)
        {
            var item = data.FirstOrDefault(x => x.BookingType == type);
            if (item == null)
                return new TypeStatisticDTO { TotalCompletedBooings = 0, TotalCanceledBookings = 0, TotalRevenue = 0 };

            return new TypeStatisticDTO
            {
                TotalCompletedBooings = item.TotalCompletedBookings,
                TotalCanceledBookings = item.TotalCanceledBookings,
                TotalRevenue = item.TotalRevenue
            };
        }

        private List<MonthlyReportDTO> MapToMonthlyReport(List<YearlyAggregateDto> data, int year)
        {
            var report = new List<MonthlyReportDTO>();
            for (int m = 1; m <= 12; m++)
            {
                var monthData = data.Where(x => x.ReportMonth == m).ToList();
                var tourData = monthData.FirstOrDefault(x => x.BookingType == BookingType.Tour);
                var accomData = monthData.FirstOrDefault(x => x.BookingType == BookingType.Accommodation);
                var comboData = monthData.FirstOrDefault(x => x.BookingType == BookingType.Combo);

                report.Add(new MonthlyReportDTO
                {
                    Month = m,
                    Year = year,
                    TourRevenue = tourData?.TotalRevenue ?? 0,
                    TourCompletedBookings = tourData?.CompletedBookings ?? 0,
                    AccommodationRevenue = accomData?.TotalRevenue ?? 0,
                    AccommodationCompletedBookings = accomData?.CompletedBookings ?? 0,
                    ComboRevenue = comboData?.TotalRevenue ?? 0,
                    ComboCompletedBookings = (decimal)(comboData?.CompletedBookings ?? 0)
                });
            }
            return report;
        }

        private class YearlyAggregateDto
        {
            public int ReportMonth { get; set; }
            public int ReportYear { get; set; }
            public BookingType BookingType { get; set; }
            public decimal TotalRevenue { get; set; }
            public int CompletedBookings { get; set; }
        }

        private class CurrentMonthAggregateDto
        {
            public BookingType BookingType { get; set; }
            public decimal TotalRevenue { get; set; }
            public int TotalBookings { get; set; }
            public int TotalCompletedBookings { get; set; }
            public int TotalCanceledBookings { get; set; }
        }

        /// <summary>
        /// Lớp DTO trung gian để hứng kết quả thô từ SQL (GIỮ NGUYÊN)
        /// </summary>
        private class RawStatisticResult
        {
            public int ItemId { get; set; }
            public string ItemCode { get; set; }
            public string ItemName { get; set; }
            public decimal rating { get; set; }
            public int totalCompletedBookings { get; set; }
            public decimal? TotalRevenue { get; set; }
            public int? totalCanceledBookings { get; set; }
            public int? TotalBookingsCount { get; set; }
            public int ItemOptionCount { get; set; }
        }
    }
}