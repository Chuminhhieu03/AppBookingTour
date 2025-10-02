//using AppBookingTour.Domain.Entities;

//namespace AppBookingTour.Application.IRepositories;

///// <summary>
///// User repository interface with specific business methods
///// </summary>
//public interface IUserRepository : IRepository<User>
//{
//    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
//    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
//    Task<bool> IsEmailExistsAsync(string email, CancellationToken cancellationToken = default);
//    Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken cancellationToken = default);
//    Task<IEnumerable<User>> GetUsersByTypeAsync(Domain.Enums.UserType userType, CancellationToken cancellationToken = default);
//    Task<IEnumerable<User>> GetRecentUsersAsync(int days = 30, CancellationToken cancellationToken = default);
//}