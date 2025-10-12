using AppBookingTour.Application.Features.Discounts.SearchDiscounts;
using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Entities;
using AppBookingTour.Share.DTOS;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;


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
            var discount = _mapper.Map<Discount>(discountDTO);
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
