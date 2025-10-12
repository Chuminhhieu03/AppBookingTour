using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Constants;
using AppBookingTour.Domain.Entities;
using MediatR;
using System.Linq.Expressions;

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
            var listDiscount = await _unitOfWork.Discounts.SearchDiscount(filter, pageIndex, pageSize);
            return new SearchDiscountResponse
            {
                Success = true,
                ListDiscount = listDiscount
            };
        }
    }
}
