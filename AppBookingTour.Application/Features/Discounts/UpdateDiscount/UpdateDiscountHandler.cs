using AppBookingTour.Application.Features.Discounts.AddNewDiscount;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Constants;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Domain.IRepositories;
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
                throw new Exception(Message.NotFound);
            }
            _mapper.Map(discountDTO, discount);
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
