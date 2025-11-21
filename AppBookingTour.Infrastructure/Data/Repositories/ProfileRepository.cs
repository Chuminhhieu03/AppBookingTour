using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
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
}