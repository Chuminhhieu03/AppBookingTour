using AppBookingTour.Application.IRepositories;
using AppBookingTour.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBookingTour.Infrastructure.Data.Repositories
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        public ImageRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<List<Image>> GetListAccommodationImageByEntityId(int? entityId)
        {
            IQueryable<Image> query = _dbSet;
            return await _dbSet
                .Where(x => x.EntityId == entityId)
                .Where(x => x.EntityType == Domain.Enums.EntityType.Accommodation)
                .OrderBy(x => x.Id)
                .ToListAsync();
        }
    }
}
