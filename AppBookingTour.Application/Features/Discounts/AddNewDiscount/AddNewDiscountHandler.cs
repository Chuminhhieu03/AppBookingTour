using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AutoMapper;
using MediatR;


namespace AppBookingTour.Application.Features.Discounts.AddNewDiscount
{
    public class AddNewDiscountHandler : IRequestHandler<AddNewDiscountCommand, AddNewDiscountResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddNewDiscountHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AddNewDiscountResponse> Handle(AddNewDiscountCommand request, CancellationToken cancellationToken)
        {
            var discountDTO = request.Discount ?? new AddNewDiscountDTO();
            var discount = _mapper.Map<Discount>(discountDTO) ?? new Discount();
            var existDiscount = _unitOfWork.Discounts.FindAsync(x => x.Code.Trim() == discount.Code.Trim());
            if (existDiscount != null)
            {
                throw new ArgumentException(string.Format("Đã tồn tại mã giảm giá [{0}]", discount.Code));
            }
            discount.RemainQuantity = discount.TotalQuantity;
            await _unitOfWork.Discounts.AddAsync(discount, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new AddNewDiscountResponse
            {
                Discount = discount,
                Success = true
            };
        }
    }
}
