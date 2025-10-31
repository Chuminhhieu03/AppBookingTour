using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Application.IRepositories
{
    public interface IImageRepository : IRepository<Image>
    {
        public Task<List<Image>> GetListImageByEntityIdAndEntityType(int? entityId, EntityType entityType);
        public Task<int> RemoveRangeByImgUrls(List<string> imageUrls);
    }
}
