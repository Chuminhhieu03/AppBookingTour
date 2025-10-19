using AppBookingTour.Domain.Constants;
using MediatR;

namespace AppBookingTour.Application.Features.Discounts.SetupDiscountAddnew
{
    public class SetupDiscountAddnewHandler : IRequestHandler<SetupDiscountAddnewQuery, SetupDiscountAddnewDTO>
    {
        public Task<SetupDiscountAddnewDTO> Handle(SetupDiscountAddnewQuery request, CancellationToken cancellationToken)
        {
            var result = new SetupDiscountAddnewDTO
            {
                ListStatus = Constants.ActiveStatus.dctName.ToList(),
                ListServiceType = Constants.ServiceTypes.dctName.ToList(),
            };
            return Task.FromResult(result);
        }
    }
}
