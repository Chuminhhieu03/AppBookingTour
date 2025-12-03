using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;

namespace AppBookingTour.Application.Features.Discounts.GetDiscountsByEntityType
{
    public class GetDiscountsByEntityTypeResponse : BaseResponse
    {
        public List<Discount>? ListDiscount { get; set; }
        public PaginationMeta? Meta { get; set; }
    }

    public class PaginationMeta
    {
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }

    public class GetDiscountsByEntityTypeFilter
    {
        public int? EntityId { get; set; }
        public int? EntityType { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
    }
}

