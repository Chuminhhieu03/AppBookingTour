using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Constants;
using MediatR;

namespace AppBookingTour.Application.Features.Discounts.SearchDiscounts
{
    public class SearchDiscountHandler : IRequestHandler<SearchDiscountQuery, SearchDiscountResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SearchDiscountHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SearchDiscountResponse> Handle(SearchDiscountQuery request, CancellationToken cancellationToken)
        {
            var filter = request.discountFilter ?? new SearchDiscountFilter();
            int pageIndex = request.pageIndex ?? Constants.Pagination.PageIndex;
            int pageSize = request.pageSize ?? Constants.Pagination.PageSize;
            
            var (listDiscount, totalCount) = await _unitOfWork.Discounts.SearchDiscount(filter, pageIndex, pageSize);
            
            listDiscount?.ForEach(item =>
            {
                if (item.Status.HasValue)
                    item.StatusName = Constants.ActiveStatus.dctName[item.Status.Value];
                if (item.ServiceType.HasValue && Constants.ServiceType.dctName.ContainsKey(item.ServiceType.Value))
                    item.ServiceTypeName = Constants.ServiceType.dctName[item.ServiceType.Value];
            });
            
            var totalPages = (pageSize == 0) ? 0 : (int)Math.Ceiling((double)totalCount / pageSize);
            
            return new SearchDiscountResponse
            {
                Success = true,
                ListDiscount = listDiscount,
                Meta = new PaginationMeta
                {
                    TotalCount = totalCount,
                    Page = pageIndex,
                    PageSize = pageSize,
                    TotalPages = totalPages
                }
            };
        }
    }
}
