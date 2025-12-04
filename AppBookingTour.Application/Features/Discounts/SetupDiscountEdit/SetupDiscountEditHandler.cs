using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Constants;
using MediatR;

namespace AppBookingTour.Application.Features.Discounts.SetupDiscountEdit
{
    public class SetupDiscountEditHandler : IRequestHandler<SetupDiscountEditQuery, SetupDiscountEditDTO>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SetupDiscountEditHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SetupDiscountEditDTO> Handle(SetupDiscountEditQuery request, CancellationToken cancellationToken)
        {
            var discount = await _unitOfWork.Discounts.GetByIdAsync(request.id);
            if (discount == null)
            {
                throw new Exception(Message.NotFound);
            }
            return new SetupDiscountEditDTO
            {
                Discount = discount,
                ListStatus = Constants.ActiveStatus.dctName.ToList(),
                ListServiceType = Constants.ServiceType.dctName.ToList(),
                Success = true
            };
        }
    }
}
