using AppBookingTour.Application.Features.Tours.SearchTours;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.Enums;

namespace AppBookingTour.Application.IRepositories;

public interface ISystemParameterRepository : IRepository<SystemParameter>
{
    Task<List<SystemParameter>> GetListSystemParameterByFeatureCode(FeatureCode featureCode);
    Task<List<SystemParameter>> GetListSystemParameterByListId(List<int> listId);

}