using AppBookingTour.Domain.Entities;

namespace AppBookingTour.Application.IRepositories;

public interface IProfileRepository
{

    Task<User?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);

    void UpdateUser(User user);

    Task<List<User>> GetGuidesAsync(CancellationToken cancellationToken = default);

    Task<Dictionary<int, string>> GetGuideNamesMapAsync(IEnumerable<int> guideIds, CancellationToken cancellationToken = default);
}