using AppBookingTour.Application.IRepositories;
using Microsoft.EntityFrameworkCore;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.Enums;
using AppBookingTour.Infrastructure.Database;

namespace AppBookingTour.Infrastructure.Data.Repositories;

public class ProfileRepository : IProfileRepository
{
    private readonly ApplicationDbContext _context;

    public ProfileRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default)
    {

        return await _context.Users
            .FindAsync(new object[] { id }, cancellationToken);
    }

    public void UpdateUser(User user)
    {
        _context.Users.Update(user);
    }

    public async Task<List<User>> GetGuidesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(u => u.UserType == UserType.Guide)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Dictionary<int, string>> GetGuideNamesMapAsync(IEnumerable<int> guideIds, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(u => guideIds.Contains(u.Id) && u.UserType == UserType.Guide)
            .ToDictionaryAsync(u => u.Id, u => u.FullName, cancellationToken);
    }
}