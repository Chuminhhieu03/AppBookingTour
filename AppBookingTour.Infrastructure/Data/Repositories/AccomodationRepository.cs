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
        public AccomodationRepository(ApplicationDbContext context) : base(context)
        {
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
                .OrderBy(x => -x.Id)
                .Skip(pageIndex * pageSize)
                .Take(pageSize);
            var listAccommodation = await query.ToListAsync();
            return (listAccommodation, totalCount);
        }

        public async Task<(List<CustomerAccommodationListItem> Results, int TotalCount)> SearchAccommodationsForCustomerAsync(
            SearchAccommodationsForCustomerFilter filter,
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Accommodations
                .Include(a => a.City)
                .Include(a => a.ListRoomType)
                    .ThenInclude(rt => rt.ListRoomInventory)
                .AsQueryable();

            // --- FILTER CƠ BẢN ---
            if (filter.CityId.HasValue)
                query = query.Where(a => a.CityId == filter.CityId);

            if (filter.Type.HasValue)
                query = query.Where(a => a.Type == filter.Type);

            if (filter.StarRating.HasValue)
                query = query.Where(a => a.StarRating == filter.StarRating);

            if (filter.PriceFrom.HasValue)
                query = query.Where(a => a.ListRoomType.Any(rt => rt.Price >= filter.PriceFrom));

            if (filter.PriceTo.HasValue)
                query = query.Where(a => a.ListRoomType.Any(rt => rt.Price <= filter.PriceTo));


            // --- LỌC ROOM INVENTORY ---
            // Yêu cầu: Accommodation phải có ít nhất 1 roomType có roomInventory
            query = query.Where(a => a.ListRoomType.Any(rt => rt.ListRoomInventory.Any()));


            // --- LỌC THEO NGÀY ---
            if (filter.CheckInDate.HasValue && filter.CheckOutDate.HasValue && filter.CheckInDate < filter.CheckOutDate)
            {
                var checkIn = filter.CheckInDate.Value.Date;
                var checkOut = filter.CheckOutDate.Value.Date;

                query = query.Where(a =>
                    a.ListRoomType.Any(rt =>
                        rt.ListRoomInventory.Any(ri =>
                            ri.Date >= checkIn &&
                            ri.Date < checkOut &&
                            ri.BookedRooms > 0
                        )
                    )
                );
            }


            // --- TÍNH TỔNG ---
            var totalCount = await query.CountAsync(cancellationToken);


            // --- PHÂN TRANG ---
            var results = await query
                .OrderBy(a => a.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new CustomerAccommodationListItem
                {
                    Id = a.Id,
                    Name = a.Name,
                    CityName = a.City.Name,
                    Type = a.Type,
                    StarRating = a.StarRating,
                    CoverImgUrl = a.CoverImgUrl,
                    MinRoomTypePrice = a.ListRoomType.Min(rt => rt.Price) ?? 0
                })
                .ToListAsync(cancellationToken);

            return (results, totalCount);
        }
    }
}
