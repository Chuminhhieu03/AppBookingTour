using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Constants;
using AutoMapper;
using MediatR;

namespace AppBookingTour.Application.Features.Discounts.UpdateDiscount
{
    public class UpdateDiscountHandler : IRequestHandler<UpdateDiscountCommand, UpdateDiscountResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateDiscountHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UpdateDiscountResponse> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
        {
            var discountDTO = request.Discount ?? new UpdateDiscountDTO();
            var discount = await _unitOfWork.Discounts.GetByIdAsync(request.DiscountId);
            if (discount == null) { 
                throw new ArgumentException(Message.NotFound);
            }
            _mapper.Map(discountDTO, discount);
            var listDiscountTmp = await _unitOfWork.Discounts.FindAsync(x => x.Code.Trim() == discount.Code.Trim());
            var existDiscount = listDiscountTmp.FirstOrDefault();
            if (existDiscount != null && existDiscount.Id != discount.Id)
            {
                throw new ArgumentException(string.Format("Đã tồn tại mã giảm giá [{0}]", discount.Code));
            }
            if (discount.TotalQuantity.HasValue && discount.RemainQuantity.HasValue && discount.TotalQuantity.Value < discount.RemainQuantity.Value)
            {
                discount.RemainQuantity = discount.TotalQuantity;
            }
            _unitOfWork.Discounts.Update(discount);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return new UpdateDiscountResponse
            {
                Discount = discount,
                Success = true
            };
        }
    }
}
