//using AppBookingTour.Application.IRepositories;
//using AppBookingTour.Domain.Entities;
//using AppBookingTour.Infrastructure.Database;
//using Microsoft.EntityFrameworkCore;

//namespace AppBookingTour.Infrastructure.Data.Repositories;

///// <summary>
///// Specific repository implementations for domain entities
///// Implement interfaces from Application layer
///// </summary>

//public class UserRepository : Repository<User>, IUserRepository
//{
//    public UserRepository(ApplicationDbContext context) : base(context) { }

//    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
//    {
//        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
//    }

//    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
//    {
//        return await _dbSet.FirstOrDefaultAsync(u => u.UserName == username, cancellationToken);
//    }

//    public async Task<bool> IsEmailExistsAsync(string email, CancellationToken cancellationToken = default)
//    {
//        return await _dbSet.AnyAsync(u => u.Email == email, cancellationToken);
//    }
//}
