namespace AppBookingTour.Application.IRepositories
{
    public interface IImageRepository : IRepository<Image>
    {
        public Task<List<Image>> GetListAccommodationImageByEntityId(int? entityId);
    }
}
