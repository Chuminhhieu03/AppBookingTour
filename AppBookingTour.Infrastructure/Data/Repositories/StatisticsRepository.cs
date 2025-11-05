using AppBookingTour.Application.Features.Statistics.ItemBookingCountDetail;
using AppBookingTour.Application.Features.Statistics.ItemRevenueDetail;
using AppBookingTour.Application.Features.Statistics.ItemStatisticByBookingCount;
using AppBookingTour.Application.Features.Statistics.ItemStatisticByRevenue;
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
                bookingType = (int)itemType
            };

            /* TODO: (Như bạn đã ghi)
             * - Cập nhật lại status (như nào là complete, như nào là cancel => khi code xong booking
             */

            switch (itemType)
            {
                case ItemType.Tour:
                    sql = @"
                        SELECT
                            T.Id AS ItemId,
                            T.Code AS ItemCode,
                            T.Name AS ItemName,
                            T.Rating AS rating,
                            COUNT(CASE WHEN B.Status IN (3, 5) THEN B.Id ELSE NULL END) AS totalCompletedBookings,
                            ISNULL(SUM(CASE WHEN B.Status IN (3, 5) THEN B.FinalAmount ELSE 0 END), 0) AS TotalRevenue,
                            COUNT(CASE WHEN B.Status IN (4, 6) THEN B.Id ELSE NULL END) AS totalCanceledBookings,
                            COUNT(CASE WHEN B.Status IN (2, 3, 4, 5, 6) THEN B.Id ELSE NULL END) AS TotalBookingsCount,
                            COUNT(DISTINCT TD.Id) AS ItemOptionCount
                        FROM
                            Bookings AS B
                        JOIN
                            TourDepartures AS TD ON B.ItemId = TD.Id
                        JOIN
                            Tours AS T ON TD.TourId = T.Id
                        WHERE
                            B.BookingType = @bookingType
                            AND B.BookingDate BETWEEN @startDate AND @endDate
                            AND B.Status IN (2, 3, 5, 4, 6)
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
                            COUNT(CASE WHEN B.Status IN (3, 5) THEN B.Id ELSE NULL END) AS totalCompletedBookings,
                            ISNULL(SUM(CASE WHEN B.Status IN (3, 5) THEN B.FinalAmount ELSE 0 END), 0) AS TotalRevenue,
                            COUNT(CASE WHEN B.Status IN (4, 6) THEN B.Id ELSE NULL END) AS totalCanceledBookings,
                            COUNT(CASE WHEN B.Status IN (2, 3, 4, 5, 6) THEN B.Id ELSE NULL END) AS TotalBookingsCount,
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
                            AND B.Status IN (2, 3, 5, 4, 6)
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
                            COUNT(CASE WHEN B.Status IN (3, 5) THEN B.Id ELSE NULL END) AS totalCompletedBookings,
                            ISNULL(SUM(CASE WHEN B.Status IN (3, 5) THEN B.FinalAmount ELSE 0 END), 0) AS TotalRevenue,
                            COUNT(CASE WHEN B.Status IN (4, 6) THEN B.Id ELSE NULL END) AS totalCanceledBookings,
                            COUNT(CASE WHEN B.Status IN (2, 3, 4, 5, 6) THEN B.Id ELSE NULL END) AS TotalBookingsCount,
                            COUNT(DISTINCT CS.Id) AS ItemOptionCount
                        FROM
                            Bookings AS B
                        JOIN
                            ComboSchedules AS CS ON B.ItemId = CS.Id
                        JOIN
                            Combos AS C ON CS.ComboId = C.Id
                        WHERE
                            B.BookingType = @bookingType
                            AND B.BookingDate BETWEEN @startDate AND @endDate
                            AND B.Status IN (2, 3, 5, 4, 6)
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
                totalCompletedBookings = r.totalCompletedBookings,
                totalCanceledBookings = r.totalCanceledBookings,
                TotalRevenue = r.TotalRevenue!.Value,
                ItemOptionCount = r.ItemOptionCount,
                rating = r.rating,
                cancellationRate = (r.TotalBookingsCount == 0) ? 0 : Math.Round((decimal)r.totalCanceledBookings / r.TotalBookingsCount, 4),
                averageRevenuePerBooking = (r.totalCompletedBookings == 0) ? 0 : r.TotalRevenue!.Value / r.totalCompletedBookings
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
                itemId
            };

            switch (itemType)
            {
                case ItemType.Tour:
                    sql = @"
                        SELECT
                            FORMAT(TD.DepartureDate, 'yyyy-MM-dd') AS ItemDetailName,
                            COUNT(CASE WHEN B.Status IN (3, 5) THEN B.Id ELSE NULL END) AS totalCompletedBookings,
                            ISNULL(SUM(CASE WHEN B.Status IN (3, 5) THEN B.FinalAmount ELSE 0 END), 0) AS TotalRevenue,
                            COUNT(CASE WHEN B.Status IN (4, 6) THEN B.Id ELSE NULL END) AS totalFailedBookings
                        FROM
                            TourDepartures AS TD
                        LEFT JOIN
                            Bookings AS B ON TD.Id = B.ItemId
                                AND B.BookingType = @bookingType
                                AND B.Status IN (2, 3, 5, 4, 6)
                                AND CAST(B.BookingDate AS DATE) BETWEEN @startDate AND @endDate
                        WHERE
                            TD.TourId = @itemId
                        GROUP BY
                            TD.Id, TD.DepartureDate
                        ORDER BY
                            TD.DepartureDate;
                    ";
                    break;

                case ItemType.Accommodation:
                    sql = @"
                        SELECT
                            RT.Name AS ItemDetailName,
                            COUNT(CASE WHEN B.Status IN (3, 5) THEN B.Id ELSE NULL END) AS totalCompletedBookings,
                            ISNULL(SUM(CASE WHEN B.Status IN (3, 5) THEN B.FinalAmount ELSE 0 END), 0) AS TotalRevenue,
                            COUNT(CASE WHEN B.Status IN (4, 6) THEN B.Id ELSE NULL END) AS totalFailedBookings
                        FROM
                            RoomTypes AS RT
                        LEFT JOIN
                            Bookings AS B ON RT.Id = B.ItemId
                                AND B.BookingType = @bookingType
                                AND B.Status IN (2, 3, 5, 4, 6)
                                AND CAST(B.BookingDate AS DATE) BETWEEN @startDate AND @endDate
                        WHERE
                            RT.AccommodationId = @itemId
                        GROUP BY
                            RT.Id, RT.Name
                        ORDER BY
                            RT.Name;
                    ";
                    break;

                case ItemType.Combo:
                    sql = @"
                        SELECT
                            FORMAT(CS.DepartureDate, 'yyyy-MM-dd') AS ItemDetailName,
                            COUNT(CASE WHEN B.Status IN (3, 5) THEN B.Id ELSE NULL END) AS totalCompletedBookings,
                            ISNULL(SUM(CASE WHEN B.Status IN (3, 5) THEN B.FinalAmount ELSE 0 END), 0) AS TotalRevenue,
                            COUNT(CASE WHEN B.Status IN (4, 6) THEN B.Id ELSE NULL END) AS totalFailedBookings
                        FROM
                            ComboSchedules AS CS
                        LEFT JOIN
                            Bookings AS B ON CS.Id = B.ItemId
                                AND B.BookingType = @bookingType
                                AND B.Status IN (2, 3, 5, 4, 6)
                                AND CAST(B.BookingDate AS DATE) BETWEEN @startDate AND @endDate
                        WHERE
                            CS.ComboId = @itemId
                        GROUP BY
                            CS.Id, CS.DepartureDate
                        ORDER BY
                            CS.DepartureDate;
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
                bookingType = (int)itemType
            };

            switch (itemType)
            {
                case ItemType.Tour:
                    sql = @"
                        SELECT
                            T.Id AS ItemId, T.Code AS ItemCode, T.Name AS ItemName, T.Rating AS rating,
                            COUNT(CASE WHEN B.Status IN (3, 5) THEN B.Id ELSE NULL END) AS totalCompletedBookings,
                            COUNT(CASE WHEN B.Status IN (4, 6) THEN B.Id ELSE NULL END) AS totalCanceledBookings,
                            COUNT(CASE WHEN B.Status IN (2, 3, 4, 5, 6) THEN B.Id ELSE NULL END) AS TotalBookingsCount,
                            COUNT(DISTINCT TD.Id) AS ItemOptionCount
                        FROM Bookings AS B
                        JOIN TourDepartures AS TD ON B.ItemId = TD.Id
                        JOIN Tours AS T ON TD.TourId = T.Id
                        WHERE B.BookingType = @bookingType
                          AND CAST(B.BookingDate AS DATE) BETWEEN @startDate AND @endDate 
                          AND B.Status IN (2, 3, 5, 4, 6)
                        GROUP BY T.Id, T.Code, T.Name, T.Rating
                    ";
                    break;

                case ItemType.Accommodation:
                    sql = @"
                        SELECT
                            A.Id AS ItemId, A.Code AS ItemCode, A.Name AS ItemName, A.Rating AS rating,
                            COUNT(CASE WHEN B.Status IN (3, 5) THEN B.Id ELSE NULL END) AS totalCompletedBookings,
                            COUNT(CASE WHEN B.Status IN (4, 6) THEN B.Id ELSE NULL END) AS totalCanceledBookings,
                            COUNT(CASE WHEN B.Status IN (2, 3, 4, 5, 6) THEN B.Id ELSE NULL END) AS TotalBookingsCount,
                            COUNT(DISTINCT RT.Id) AS ItemOptionCount 
                        FROM Bookings AS B
                        JOIN RoomTypes AS RT ON B.ItemId = RT.Id 
                        JOIN Accommodations AS A ON RT.AccommodationId = A.Id
                        WHERE B.BookingType = @bookingType
                          AND CAST(B.BookingDate AS DATE) BETWEEN @startDate AND @endDate 
                          AND B.Status IN (2, 3, 5, 4, 6)
                        GROUP BY A.Id, A.Code, A.Name, A.Rating
                    ";
                    break;

                case ItemType.Combo:
                    sql = @"
                        SELECT
                            C.Id AS ItemId, C.Code AS ItemCode, C.Name AS ItemName, C.Rating AS rating,
                            COUNT(CASE WHEN B.Status IN (3, 5) THEN B.Id ELSE NULL END) AS totalCompletedBookings,
                            COUNT(CASE WHEN B.Status IN (4, 6) THEN B.Id ELSE NULL END) AS totalCanceledBookings,
                            COUNT(CASE WHEN B.Status IN (2, 3, 4, 5, 6) THEN B.Id ELSE NULL END) AS TotalBookingsCount,
                            COUNT(DISTINCT CS.Id) AS ItemOptionCount
                        FROM Bookings AS B
                        JOIN ComboSchedules AS CS ON B.ItemId = CS.Id
                        JOIN Combos AS C ON CS.ComboId = C.Id
                        WHERE B.BookingType = @bookingType
                          AND CAST(B.BookingDate AS DATE) BETWEEN @startDate AND @endDate 
                          AND B.Status IN (2, 3, 5, 4, 6)
                        GROUP BY C.Id, C.Code, C.Name, C.Rating
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
                totalCanceledBookings = r.totalCanceledBookings,
                ItemOptionCount = r.ItemOptionCount,
                rating = r.rating,
                cancellationRate = (r.TotalBookingsCount == 0) ? 0 : Math.Round((decimal)r.totalCanceledBookings / r.TotalBookingsCount, 4)
            })
            .OrderByDescending(r => r.totalCompletedBookings)
            .ToList();

            return finalResults;
        }

        public async Task<IEnumerable<ItemBookingCountDetailDTO>>    GetItemBookingCountDetailAsync(
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
                itemId
            };

            switch (itemType)
            {
                case ItemType.Tour:
                    sql = @"
                        SELECT
                            FORMAT(TD.DepartureDate, 'yyyy-MM-dd') AS ItemDetailName,
                            COUNT(CASE WHEN B.Status IN (2, 3, 5) THEN B.Id ELSE NULL END) AS totalCompletedBookings,
                            COUNT(CASE WHEN B.Status IN (4, 6) THEN B.Id ELSE NULL END) AS totalCanceledBookings
                        FROM
                            TourDepartures AS TD
                        LEFT JOIN
                            Bookings AS B ON TD.Id = B.ItemId
                                AND B.BookingType = @bookingType
                                AND B.Status IN (2, 3, 5, 4, 6)
                                AND CAST(B.BookingDate AS DATE) BETWEEN @startDate AND @endDate
                        WHERE
                            TD.TourId = @itemId
                        GROUP BY
                            TD.Id, TD.DepartureDate
                        ORDER BY
                            TD.DepartureDate;
                    ";
                    break;

                case ItemType.Accommodation:
                    sql = @"
                        SELECT
                            RT.Name AS ItemDetailName,
                            COUNT(CASE WHEN B.Status IN (2, 3, 5) THEN B.Id ELSE NULL END) AS totalCompletedBookings,
                            COUNT(CASE WHEN B.Status IN (4, 6) THEN B.Id ELSE NULL END) AS totalCanceledBookings
                        FROM
                            RoomTypes AS RT
                        LEFT JOIN
                            Bookings AS B ON RT.Id = B.ItemId
                                AND B.BookingType = @bookingType
                                AND B.Status IN (2, 3, 5, 4, 6)
                                AND CAST(B.BookingDate AS DATE) BETWEEN @startDate AND @endDate
                        WHERE
                            RT.AccommodationId = @itemId
                        GROUP BY
                            RT.Id, RT.Name
                        ORDER BY
                            RT.Name;
                    ";
                    break;

                case ItemType.Combo:
                    sql = @"
                        SELECT
                            FORMAT(CS.DepartureDate, 'yyyy-MM-dd') AS ItemDetailName,
                            COUNT(CASE WHEN B.Status IN (2, 3, 5) THEN B.Id ELSE NULL END) AS totalCompletedBookings,
                            COUNT(CASE WHEN B.Status IN (4, 6) THEN B.Id ELSE NULL END) AS totalCanceledBookings
                        FROM
                            ComboSchedules AS CS
                        LEFT JOIN
                            Bookings AS B ON CS.Id = B.ItemId
                                AND B.BookingType = @bookingType
                                AND B.Status IN (2, 3, 5, 4, 6)
                                AND CAST(B.BookingDate AS DATE) BETWEEN @startDate AND @endDate
                        WHERE
                            CS.ComboId = @itemId
                        GROUP BY
                            CS.Id, CS.DepartureDate
                        ORDER BY
                            CS.DepartureDate;
                    ";
                    break;

                default:
                    return Enumerable.Empty<ItemBookingCountDetailDTO>();
            }

            var results = await _context.Database.GetDbConnection()
                .QueryAsync<ItemBookingCountDetailDTO>(sql, parameters);

            return results;
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
            public int totalCanceledBookings { get; set; }
            public int TotalBookingsCount { get; set; }
            public int ItemOptionCount { get; set; }
        }
    }
}