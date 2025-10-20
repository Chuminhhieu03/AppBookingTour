using AppBookingTour.Application.IRepositories;
using AppBookingTour.Domain.Constants;
using MediatR;

namespace AppBookingTour.Application.Features.Discounts.SetupDiscountDisplay
{
    public class SetupDiscountDisplayHandler : IRequestHandler<SetupDiscountDisplayQuery, SetupDiscountDisplayDTO>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SetupDiscountDisplayHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SetupDiscountDisplayDTO> Handle(SetupDiscountDisplayQuery request, CancellationToken cancellationToken)
        {
            var discount = await _unitOfWork.Discounts.GetByIdAsync(request.id);
            if (discount == null)
            {
                throw new Exception(Message.NotFound);
            }
            if (discount.Status.HasValue)
                discount.StatusName = Constants.ActiveStatus.dctName[discount.Status.Value];
            if (discount.ServiceType.HasValue)
                discount.ServiceTypeName = Constants.ServiceType.dctName[discount.ServiceType.Value];
            return new SetupDiscountDisplayDTO
            {
                Discount = discount,
                Success = true
            };
        }
    }
}
