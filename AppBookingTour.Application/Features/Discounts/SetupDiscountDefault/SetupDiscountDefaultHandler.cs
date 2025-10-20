using AppBookingTour.Domain.Constants;
using MediatR;

namespace AppBookingTour.Application.Features.Discounts.SetupDiscountDefault
{
    public class SetupDiscountDefaultHandler : IRequestHandler<SetupDiscountDefaultQuery, SetupDiscountDefaultDTO>
    {
        public async Task<SetupDiscountDefaultDTO> Handle(SetupDiscountDefaultQuery request, CancellationToken cancellationToken)
        {
            return new SetupDiscountDefaultDTO
            {
                ListStatus = Constants.ActiveStatus.dctName.ToList(),
                Success = true
            };
        }
    }
}
