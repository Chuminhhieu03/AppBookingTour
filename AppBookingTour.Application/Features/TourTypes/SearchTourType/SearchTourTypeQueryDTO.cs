using AppBookingTour.Application.Features.Tours.SearchTours;
using AppBookingTour.Application.Features.TourTypes.GetTourTypeById;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.TourTypes.SearchTourType;

public class TourTypeFilter
{
    public string? Name { get; set; }
}

public class SearchTourTypesResponse : BaseResponse
{
    public List<TourTypeDTO> TourTypes { get; set; } = [];
    public PaginationMeta Meta { get; set; } = null!;
}
