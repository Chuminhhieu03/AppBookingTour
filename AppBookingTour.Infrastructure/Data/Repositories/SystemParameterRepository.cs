using AppBookingTour.Application.Features.Tours.SearchTours;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.Enums;
using AppBookingTour.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace AppBookingTour.Infrastructure.Data.Repositories;

public class SystemParameterRepository : Repository<SystemParameter>, ISystemParameterRepository
{
    public SystemParameterRepository(ApplicationDbContext context) : base(context) { }

    public async Task<List<SystemParameter>> GetListSystemParameterByFeatureCode(FeatureCode featureCode)
    {
        IQueryable<SystemParameter> query = _dbSet;
        return await _dbSet.Where(x => x.FeatureCode == featureCode).ToListAsync();

    }

    public async Task<List<SystemParameter>> GetListSystemParameterByListId(List<int> listId)
    {
        IQueryable<SystemParameter> query = _dbSet;
        return await _dbSet.Where(x => listId.Contains(x.Id)).ToListAsync();
    }
}
