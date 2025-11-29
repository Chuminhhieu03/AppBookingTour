using AppBookingTour.Application.Features.Accommodations.SearchAccommodation;
using AppBookingTour.Application.Features.Accommodations.SearchAccommodationsForCustomer;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Infrastructure.Database;
using Dapper;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AppBookingTour.Infrastructure.Data.Repositories
{
    public class AccomodationRepository : Repository<Accommodation>, IAccommodationRepository
    {
        private readonly ApplicationDbContext _context;
        public AccomodationRepository(ApplicationDbContext context) : base(context) {
            _context = context;
        }

        public async Task<Accommodation> GetById(int id)
        {
            IQueryable<Accommodation> query = _dbSet
                .Include(x => x.City)
                .Include(x => x.ListRoomType);
            return await query.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<(List<Accommodation> ListAccommodation, int TotalCount)> SearchAccommodation(SearchAccommodationFilter accommodationFilter, int pageIndex, int pageSize)
        {
            IQueryable<Accommodation> query = _dbSet.Include(x => x.City);
            if (!string.IsNullOrEmpty(accommodationFilter.Name))
                query = query.Where(x => x.Name.Contains(accommodationFilter.Name));
            if (!string.IsNullOrEmpty(accommodationFilter.Code))
                query = query.Where(x => x.Code.Contains(accommodationFilter.Code));
            if (accommodationFilter.IsActive.HasValue)
                query = query.Where(x => x.IsActive == accommodationFilter.IsActive.Value);
            if (accommodationFilter.Type.HasValue)
                query = query.Where(x => x.Type == accommodationFilter.Type.Value);
            if (accommodationFilter.CityId.HasValue)
                query = query.Where(x => x.CityId == accommodationFilter.CityId);
            int totalCount = await query.CountAsync();
            query = query
                .OrderBy(x => - x.Id)
                .Skip(pageIndex * pageSize)
                .Take(pageSize);
            var listAccommodation = await query.ToListAsync();
            return (listAccommodation, totalCount);
        }

        public async Task<(List<CustomerAccommodationListItem> Results, int TotalCount)> SearchAccommodationsForCustomerAsync(
            SearchAccommodationsForCustomerFilter filter, int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            var sql = GetSearchAccommodationSql();

            // Tạo các tham số (Parameters) - Rất quan trọng để chống SQL Injection
            var parameters = new DynamicParameters();
            parameters.Add("@CityId", filter.CityId);
            parameters.Add("@Type", filter.Type.HasValue ? (int)filter.Type.Value : null);
            parameters.Add("@StarRating", filter.StarRating);
            parameters.Add("@PriceFrom", filter.PriceFrom);
            parameters.Add("@PriceTo", filter.PriceTo);
            parameters.Add("@NumOfAdult", filter.NumOfAdult ?? 1);
            parameters.Add("@NumOfChild", filter.NumOfChild ?? 0);
            parameters.Add("@NumOfRoom", filter.NumOfRoom ?? 1);

            // Xử lý tham số ngày
            if (filter.CheckInDate.HasValue && filter.CheckOutDate.HasValue && filter.CheckInDate < filter.CheckOutDate)
            {
                parameters.Add("@CheckInDate", filter.CheckInDate.Value);
                parameters.Add("@CheckOutDate", filter.CheckOutDate.Value);
            }
            else
            {
                parameters.Add("@CheckInDate", null);
                parameters.Add("@CheckOutDate", null);
            }

            // Tham số phân trang
            parameters.Add("@PageIndex", pageIndex);
            parameters.Add("@PageSize", pageSize);
            parameters.Add("@Offset", (pageIndex - 1) * pageSize);

            using (var connection = _context.Database.GetDbConnection())
            {
                if (connection.State != ConnectionState.Open)
                {
                    await connection.OpenAsync(cancellationToken);
                }

                using (var multi = await connection.QueryMultipleAsync(sql, parameters))
                {
                    var totalCount = await multi.ReadSingleAsync<int>();
                    var accommodations = (await multi.ReadAsync<CustomerAccommodationListItem>()).ToList();

                    return (accommodations, totalCount);
                }
            }
        }

        private string GetSearchAccommodationSql()
        {
            return @"
            IF @CheckInDate IS NULL OR @CheckOutDate IS NULL OR @CheckInDate >= @CheckOutDate
                BEGIN
    
                    ;WITH CTE_ValidRoomTypes AS (
                        SELECT
                            rt.AccommodationId,
                            ISNULL(rt.Quantity, 0) AS Quantity,
                            rt.Price
                        FROM
                            [dbo].[RoomTypes] rt
                        WHERE
                            ISNULL(rt.MaxAdult, 0) >= @NumOfAdult
                            AND ISNULL(rt.MaxChildren, 0) >= @NumOfChild
                            AND (@PriceFrom IS NULL OR rt.Price >= @PriceFrom)
                            AND (@PriceTo IS NULL OR rt.Price <= @PriceTo)
                    ),
                    CTE_AccommodationStock AS (
                        SELECT
                            AccommodationId,
                            SUM(Quantity) AS TotalAvailableRooms,
                            MIN(Price) AS MinRoomTypePrice
                        FROM
                            CTE_ValidRoomTypes
                        GROUP BY
                            AccommodationId
                    )
                    SELECT
                        A.Id, A.Name, A.Type, A.Address, A.StarRating, A.Rating, A.CoverImgUrl, A.Code, A.Amenities,
                        C.Name AS CityName,
                        S.TotalAvailableRooms,
                        S.MinRoomTypePrice
                    INTO
                        #TempResult_NoDate
                    FROM
                        [dbo].[Accommodations] A
                    INNER JOIN
                        CTE_AccommodationStock S ON A.Id = S.AccommodationId
                    LEFT JOIN
                        [dbo].[Cities] C ON A.CityId = C.Id
                    WHERE
                        A.IsActive = 1
                        AND (@CityId IS NULL OR A.CityId = @CityId)
                        AND (@Type IS NULL OR A.Type = @Type)
                        AND (@StarRating IS NULL OR A.StarRating = @StarRating)
                        AND S.TotalAvailableRooms >= @NumOfRoom;

                    SELECT COUNT(*) AS TotalCount FROM #TempResult_NoDate;

                    SELECT *
                    FROM #TempResult_NoDate
                    ORDER BY Id DESC
                    OFFSET @Offset ROWS
                    FETCH NEXT @PageSize ROWS ONLY;

                    DROP TABLE #TempResult_NoDate;

                END
             ELSE
                BEGIN
    
                    ;WITH DateSeries AS (
                        SELECT CAST(@CheckInDate AS DATE) AS [Date]
                        UNION ALL
                        SELECT CAST(DATEADD(day, 1, [Date]) AS DATE)
                        FROM DateSeries
                        WHERE [Date] < CAST(DATEADD(day, -1, @CheckOutDate) AS DATE)
                    ),
                    RoomTypeDailyAvailability AS (
                        SELECT
                            rt.Id AS RoomTypeId,
                            rt.AccommodationId,
                            rt.Quantity,
                            rt.MaxAdult,
                            rt.MaxChildren,
                            rt.Price,
                            CASE 
                                WHEN ri.RoomTypeId IS NULL THEN 0 
                                ELSE (ISNULL(rt.Quantity, 0) - ISNULL(ri.BookedRooms, 0)) 
                            END AS DailyAvailableStock
                        FROM
                            [dbo].[RoomTypes] rt
                        CROSS JOIN 
                            DateSeries ds
                        LEFT JOIN 
                            [dbo].[RoomInventories] ri ON rt.Id = ri.RoomTypeId 
                                             AND CAST(ri.[Date] AS DATE) = ds.[Date]
                    ),
                    MinStockPerRoomType AS (
                        SELECT
                            RoomTypeId,
                            AccommodationId,
                            MaxAdult,
                            MaxChildren,
                            Price,
                            MIN(DailyAvailableStock) AS MinAvailableStock
                        FROM RoomTypeDailyAvailability
                        GROUP BY
                            RoomTypeId, AccommodationId, Quantity, MaxAdult, MaxChildren, Price
                    ),
                    ValidRoomTypes AS (
                        SELECT *
                        FROM MinStockPerRoomType
                        WHERE
                            MinAvailableStock > 0
                            AND ISNULL(MaxAdult, 0) >= @NumOfAdult
                            AND ISNULL(MaxChildren, 0) >= @NumOfChild
                            AND (@PriceFrom IS NULL OR Price >= @PriceFrom)
                            AND (@PriceTo IS NULL OR Price <= @PriceTo)
                    ),
                    AccommodationStock AS (
                        SELECT
                            AccommodationId,
                            SUM(MinAvailableStock) AS TotalAvailableRooms,
                            MIN(Price) AS MinRoomTypePrice
                        FROM ValidRoomTypes
                        GROUP BY
                            AccommodationId
                    )
                    SELECT
                        A.Id, A.Name, A.Type, A.Address, A.StarRating, A.Rating, A.CoverImgUrl, A.Code, A.Amenities,
                        C.Name AS CityName,
                        ast.TotalAvailableRooms,
                        ast.MinRoomTypePrice
                    INTO
                        #TempResult_WithDate
                    FROM
                        [dbo].[Accommodations] A
                    INNER JOIN
                        AccommodationStock ast ON A.Id = ast.AccommodationId
                    LEFT JOIN
                        [dbo].[Cities] C ON A.CityId = C.Id
                    WHERE
                        A.IsActive = 1
                        AND (@CityId IS NULL OR A.CityId = @CityId)
                        AND (@Type IS NULL OR A.Type = @Type)
                        AND (@StarRating IS NULL OR A.StarRating = @StarRating)
                        AND ast.TotalAvailableRooms >= @NumOfRoom;

                    SELECT COUNT(*) AS TotalCount FROM #TempResult_WithDate;

                    SELECT *
                    FROM #TempResult_WithDate
                    ORDER BY Id DESC
                    OFFSET @Offset ROWS
                    FETCH NEXT @PageSize ROWS ONLY;

                    DROP TABLE #TempResult_WithDate;

                END
            ";
        }
    }
}
